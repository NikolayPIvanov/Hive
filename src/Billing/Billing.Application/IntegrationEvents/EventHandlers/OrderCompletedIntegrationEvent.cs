using System;
using System.Linq;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using DotNetCore.CAP;
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
        private readonly ILogger<OrderCompletedIntegrationEventHandler> _logger;

        public OrderCompletedIntegrationEventHandler(IBillingDbContext context, ILogger<OrderCompletedIntegrationEventHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
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

            var alreadyPaid =buyerAccount.Transactions.Any(t =>
                t.OrderNumber == @event.OrderNumber && t.TransactionType == TransactionType.Payment);

            if (!alreadyPaid)
            {
                throw new Exception();
            }
            
            // add billing here for investors
            
            
            // main
            sellerAccount.AddTransaction(new Transaction(@event.BasePrice, @event.OrderNumber, TransactionType.Fund));
            await _context.SaveChangesAsync(default);
            
        }
    }
}