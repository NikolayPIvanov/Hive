using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using FluentValidation;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Security;
using Hive.Domain.Entities.Billing;
using Hive.Domain.Entities.Gigs;
using Hive.Domain.Entities.Orders;
using Hive.Domain.Enums;
using Hive.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Transaction = Hive.Domain.Entities.Billing.Transaction;

namespace Hive.Application.Ordering.Orders.Commands
{
    [Authorize(Roles = "Buyer, Administrator")]
    public record PlaceOrderCommand(string Requirements, int GigId, int PackageId) : IRequest<Guid>;

    public class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
    {
        public PlaceOrderCommandValidator(IApplicationDbContext context)
        {
            RuleFor(x => x.Requirements)
                .MinimumLength(3).WithMessage("{PropertyName} must be above {MinimumLength}")
                .MaximumLength(2000).WithMessage("{PropertyName} must be below {MaximumLength}")
                .NotEmpty().WithMessage("A {PropertyName} must be provided");
            
            RuleFor(x => x.PackageId)
                .NotNull().WithMessage("A {PropertyName} must be provided");
            
            RuleFor(x => new {x.GigId, x.PackageId})
                .MustAsync(async (id, token) => 
                    await context.Gigs
                        .Include(g => g.Packages)
                        .AnyAsync(g => g.Id == id.GigId && g.Packages.FirstOrDefault(p => p.Id == id.PackageId) != null, token))
                .WithMessage("Must specify correct gig id and package id pair")
                .NotNull().WithMessage("A {PropertyName} must be provided");
        }
    }
    
    public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public PlaceOrderCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        
        public async Task<Guid> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            var buyerId = await GetBuyerAsync(cancellationToken);
            var gig = await GetGigAsync(request, cancellationToken);

            if (gig.IsDraft)
            {
                throw new Exception("Cannot place order on draft gigs");
            }
            
            var package = gig.Packages.Single(x => x.Id == request.PackageId);
            
            var buyerAccount =
                await _context.AccountHolders
                    .Include(x => x.Wallet)
                    .ThenInclude(w => w.Transactions)
                    .FirstOrDefaultAsync(x => x.UserId == _currentUserService.UserId,
                    cancellationToken);

            if (buyerAccount == null)
            {
                throw new NotFoundException(nameof(AccountHolder));
            }
            
            // todo: keep balance in wallet
            if (!buyerAccount.Wallet.Transactions.Any())
            {
                throw new Exception("No transactions found");
            }
            
            var balance = 0.0m;
            foreach (var t in buyerAccount.Wallet.Transactions)
            {
                var amount =
                    t.TransactionType switch
                    {
                        TransactionType.Expense => t.Amount * (-1.0m),
                        TransactionType.Fund => t.Amount
                    };
                balance += amount;
            }

            var order = new Order(package.Price, request.Requirements, request.PackageId, buyerId, gig.SellerId);

            if (balance < package.Price)
            {
                throw new Exception();
            }

            order.OrderStates.Add(new State(OrderState.UserBalanceValid, string.Empty));
            order.OrderStates.Add(new State(OrderState.OrderDataValid, string.Empty));
            
            var transaction = new Transaction(package.Price, order.OrderNumber, TransactionType.Expense,
                buyerAccount.WalletId);
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync(cancellationToken);
            
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            return order.OrderNumber;
        }

        private async Task<Gig> GetGigAsync(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            var gig = await _context.Gigs
                .Include(x => x.Packages)
                .FirstOrDefaultAsync(x => x.Id == request.GigId, cancellationToken);
            if (gig == null)
            {
                throw new NotFoundException(nameof(Gig), request.GigId);
            }

            return gig;
        }

        private async Task<int> GetBuyerAsync(CancellationToken cancellationToken)
        {
            var buyer = await _context.Buyers.Select(b => new {b.Id, b.UserId})
                .FirstOrDefaultAsync(b => b.UserId == _currentUserService.UserId, cancellationToken: cancellationToken);

            if (buyer == null)
            {
                throw new NotFoundException(nameof(Buyer));
            }

            return buyer.Id;
        }
    }
}