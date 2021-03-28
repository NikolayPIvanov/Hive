using System;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using Billing.Domain;
using DotNetCore.CAP;
using Hive.Billing.Contracts.IntegrationEvents;
using Hive.Common.Application.Interfaces;
using Hive.Gig.Contracts.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Ordering.Contracts.IntegrationEvents;

namespace Billing.Application.IntegrationEvents
{
    public class OrderBillingConfirmationIntegrationEventHandler : ICapSubscribe
    {
        private readonly IBillingContext _context;
        private readonly IIntegrationEventPublisher _publisher;

        public OrderBillingConfirmationIntegrationEventHandler(IBillingContext context, IIntegrationEventPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }
        
        [CapSubscribe(nameof(OrderCreatedIntegrationEvent), Group = "cap.hive.billing")]
        public async Task Handle(OrderCreatedIntegrationEvent @event)
        {
            var userId = @event.OrderedBy;
            
            // Need to implement uplock
            var accountAndBalance = await _context.Accounts
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.UserId == userId);

            if (accountAndBalance == null)
            {
                var z = new OrderBalanceInvalidIntegrationEvent(@event.OrderNumber, "User account does not exist");
                await _publisher.Publish(z);
                return;
            }

            var transactions = accountAndBalance.Transactions;

            // This needs to go into a snapshot
            var balance = 0.0m;
            foreach (var transaction in transactions)
            {
                switch (transaction.TransactionType)
                {
                    case TransactionType.Deposit: balance += transaction.Amount;
                        break;
                    case TransactionType.Hold: balance -= transaction.Amount;
                        break;
                    case TransactionType.Payment: balance -= transaction.Amount;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (balance < 0)
            {
                throw new Exception("");
            }
            
            if (balance < @event.UnitPrice)
            {
                var x = new OrderBalanceInvalidIntegrationEvent(@event.OrderNumber, "User account does not have enough resources.");
                await _publisher.Publish(x);
                return;
            }
            
            var @ev = new OrderBalanceConfirmationIntegrationEvent(@event.OrderNumber, "User has enough balance");
            await _publisher.Publish(@ev);
            // put on hold the given amount
            
        }
    }
}