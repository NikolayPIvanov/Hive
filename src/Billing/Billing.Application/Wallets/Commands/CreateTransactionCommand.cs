using System;
using System.Threading;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using FluentValidation;
using Hive.Billing.Domain.Entities;
using Hive.Billing.Domain.Enums;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Billing.Application.Wallets.Commands
{
    public record CreateTransactionCommand(decimal Amount, Guid? OrderNumber, int WalletId) : IRequest<int>;

    public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionCommandValidator(IBillingDbContext billingDbContext, ICurrentUserService currentUserService)
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0.0m).WithMessage("{Property} be above {ComparisonValue}");
    
            RuleFor(x => x.OrderNumber)
                .Must(x => x != Guid.Empty).When(x => x.OrderNumber.HasValue)
                .WithMessage("{Property} cannot be a default one");

            RuleFor(x => x.WalletId)
                .MustAsync(async (id, token) =>
                {
                    var wallet = await billingDbContext.Wallets
                        .AsNoTracking()
                        .Include(x => x.AccountHolder)
                        .FirstOrDefaultAsync(x => x.Id == id, token);
                    return wallet == null || wallet.AccountHolder.UserId == currentUserService.UserId;
                })
                .WithMessage("Can only fund own wallets");
        }
    }
    
    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, int>
    {
        private readonly IBillingDbContext _context;
        private readonly ILogger<CreateTransactionCommandHandler> _logger;

        public CreateTransactionCommandHandler(IBillingDbContext context, ILogger<CreateTransactionCommandHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<int> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _context.Wallets
                .FirstOrDefaultAsync(w => w.Id == request.WalletId, cancellationToken);
            
            if (wallet == null)
            {
                _logger.LogWarning("Wallet with id: {@Id} does not exist", request.WalletId);
                throw new NotFoundException(nameof(Wallet), request.WalletId);
            } 
            
            var transaction = new Transaction(request.Amount, request.OrderNumber, TransactionType.Fund);
            wallet.AddTransaction(transaction);

            await _context.SaveChangesAsync(cancellationToken);

            return transaction.TransactionNumber;
        }
    }
}