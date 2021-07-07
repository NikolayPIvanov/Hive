using System;
using System.Linq;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using DotNetCore.CAP;
using Hive.Billing.Domain.Entities;
using Hive.Billing.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Contracts.IntegrationEvents;

namespace Billing.Application.IntegrationEvents.EventHandlers
{
    public class OrderCanceledIntegrationEventHandler : ICapSubscribe
    {
        private readonly IBillingDbContext _context;
        private readonly ILogger<OrderCanceledIntegrationEventHandler> _logger;

        public OrderCanceledIntegrationEventHandler(IBillingDbContext context, ILogger<OrderCanceledIntegrationEventHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        [CapSubscribe(nameof(OrderCanceledIntegrationEvent), Group = "cap.hive.billing")]
        public async Task Handle(OrderCanceledIntegrationEvent @event)
        {
            var wallet = await _context.Wallets
                .Include(w => w.AccountHolder)
                .Include(w => w.Transactions)
                .FirstOrDefaultAsync(w => w.AccountHolder.UserId == @event.CanceledBy);
            
            if(wallet == null) return;
            
            var paymentTransaction = wallet.Transactions.FirstOrDefault(t =>
                t.OrderNumber == @event.OrderNumber && t.TransactionType == TransactionType.Payment);
            
            wallet.AddTransaction(new Transaction(paymentTransaction.Amount, @event.OrderNumber, TransactionType.Fund));

            await _context.SaveChangesAsync(default);
        }
    }
}