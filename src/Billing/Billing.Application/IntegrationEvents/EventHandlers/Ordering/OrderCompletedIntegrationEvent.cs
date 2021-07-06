using System;
using System.Linq;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using BuildingBlocks.Core.Interfaces;
using DotNetCore.CAP;
using Hive.Billing.Contracts.IntegrationEvents;
using Hive.Billing.Domain.Entities;
using Hive.Billing.Domain.Enums;
using Hive.Common.Core.Exceptions;
using Hive.Identity.Contracts.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Contracts.IntegrationEvents;

namespace Billing.Application.IntegrationEvents.EventHandlers
{
    public class OrderCompletedIntegrationEventHandler  : ICapSubscribe
    {
        private readonly IBillingDbContext _context;
        private readonly IIntegrationEventPublisher _publisher;
        private readonly ILogger<OrderCompletedIntegrationEventHandler> _logger;

        public OrderCompletedIntegrationEventHandler(IBillingDbContext context, IIntegrationEventPublisher publisher, ILogger<OrderCompletedIntegrationEventHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        [CapSubscribe(nameof(OrderCompletedIntegrationEvent), Group = "cap.hive.billing")]
        public async Task Handle(OrderCompletedIntegrationEvent @event)
        {
            var buyerAccount = await _context.Wallets
                .Include(w => w.Transactions)
                .Include(a => a.AccountHolder)
                .FirstOrDefaultAsync(h => h.AccountHolder.UserId == @event.BuyerUserId);
            
            if (buyerAccount == null)
            {
                throw new NotFoundException(nameof(AccountHolder), @event.BuyerUserId);
            }
            
            var sellerAccount = await _context.Wallets
                .Include(w => w.Transactions)
                .Include(a => a.AccountHolder)
                .FirstOrDefaultAsync(h => h.AccountHolder.UserId  == @event.SellerUserId);
            
            if (sellerAccount == null)
            {
                throw new NotFoundException(nameof(AccountHolder), @event.SellerUserId);
            }

            var userIds = @event.Data.Select(d => d.UserId);
            var accounts = await _context.Wallets
                .Include(w => w.Transactions)
                .Include(a => a.AccountHolder)
                .Where(h => userIds.Contains(h.AccountHolder.UserId))
                .ToListAsync();

            var valid = true;
            // add billing here for investors
            foreach (var data in @event.Data)
            {
                var wallet = accounts.FirstOrDefault(a => a.AccountHolder.UserId == data.UserId);
                if (wallet == null)
                {
                    valid = false;
                    break;
                }
                wallet.AddTransaction(new Transaction(data.Amount, @event.OrderNumber, TransactionType.Fund));
            }

            if (valid)
            {
                await _context.SaveChangesAsync(default);
                await _publisher.PublishAsync(new OrderFundsDistributedIntegrationEvent(@event.OrderNumber, @event.ResolutionId));
            }

        }
    }
}