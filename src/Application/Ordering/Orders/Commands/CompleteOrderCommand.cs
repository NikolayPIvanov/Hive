using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Billing;
using Hive.Domain.Enums;
using Hive.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Application.Ordering.Orders.Commands
{
    public record CompleteOrderCommand(Guid OrderNumber) : IRequest;

    public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CompleteOrderCommand> _logger;

        public CompleteOrderCommandHandler(IApplicationDbContext context, ILogger<CompleteOrderCommand> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task<Unit> Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Include(o => o.Seller)
                .Include(o => o.Package)
                .FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber,
                cancellationToken);
            if (order == null)
            {
                _logger.LogError("Order {@OrderId} was not found.", request.OrderNumber);
                return Unit.Value;
            }
            order.OrderStates.Add(new State(OrderState.Completed, string.Empty));

            var sellerAccountHolder = await _context.AccountHolders
                .Include(ah => ah.Wallet)
                .FirstOrDefaultAsync(x => x.UserId == order.Seller.UserId, cancellationToken);

            var planId = await _context.Gigs
                .Select(g => new {g.PlanId, g.Id})
                .FirstOrDefaultAsync(x => x.Id == order.Package.GigId, cancellationToken);

            var totalInInvestors = 0.0m;
            if (planId?.PlanId != null)
            {
                var plan = await _context.Plans
                    .Include(p => p.Investments)
                    .ThenInclude(i => i.Investor)
                    .FirstOrDefaultAsync(x => x.Id == planId.PlanId.Value, cancellationToken);

                var roiPerInvestor = plan.Investments
                    .Where(i => i.IsAccepted)
                    .Select(x => new {x.Investor.UserId, x.RoiPercentage}).ToList();

                var investorsAccounts = await _context.AccountHolders
                    .Include(ah => ah.Wallet)
                    .Where(ah => roiPerInvestor.Any(x => x.UserId == ah.UserId))
                    .ToListAsync(cancellationToken);
                
                foreach (var accountHolder in investorsAccounts)
                {
                    var wallet = accountHolder.Wallet;
                    var roi = roiPerInvestor.Single(x => x.UserId == accountHolder.UserId);
                    var value = (decimal)roi.RoiPercentage * order.UnitPrice;
                    totalInInvestors += value;
                    wallet.Transactions.Add(new Transaction(value, order.OrderNumber, TransactionType.Fund, wallet.Id));
                }
            }

            var walletId = sellerAccountHolder.Wallet.Id;
            var transaction = new Transaction(order.UnitPrice - totalInInvestors, order.OrderNumber, TransactionType.Fund, walletId);
            _context.Transactions.Add(transaction);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}