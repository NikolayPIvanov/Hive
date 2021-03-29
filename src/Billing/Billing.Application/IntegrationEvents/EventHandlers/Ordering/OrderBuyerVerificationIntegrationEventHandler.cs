using System;
using System.Linq;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using Billing.Domain;
using DotNetCore.CAP;
using Hive.Billing.Contracts.IntegrationEvents;
using Hive.Common.Application.Interfaces;
using Hive.Gig.Contracts.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Ordering.Contracts.IntegrationEvents;

namespace Billing.Application.IntegrationEvents.EventHandlers.Ordering
{
    public class OrderBuyerVerificationIntegrationEventHandler : ICapSubscribe
    {
        private readonly IBillingContext _context;
        private readonly IIntegrationEventPublisher _publisher;

        public OrderBuyerVerificationIntegrationEventHandler(IBillingContext context, IIntegrationEventPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }
        
        [CapSubscribe(nameof(OrderPlacedIntegrationEvent), Group = "cap.hive.billing")]
        public async Task Handle(OrderPlacedIntegrationEvent @event)
        {
            var accountHolderId = @event.UserId;
            
            // Need to implement uplock
            var account = await _context.AccountHolders
                .Select(ah => new
                {
                    ah.UserId,
                    ah.Account.Id,
                    ah.Account.Transactions,
                })
                .FirstOrDefaultAsync(a => a.UserId == accountHolderId);
            
            var integrationEvent = new BuyerBalanceVerifiedIntegrationEvent(@event.OrderNumber, "User account does not exist", IsValid: false);

            if (account == null)
            {
                await _publisher.Publish(integrationEvent);
                return;
            }

            var transactions = account.Transactions;

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
                integrationEvent = integrationEvent with {Reason = "User account does not have enough resources."};
                await _publisher.Publish(integrationEvent);
                return;
            }

            var mockPaymentId = Guid.NewGuid();
            var newTransaction = new Transaction(TransactionType.Hold, @event.UnitPrice, @event.OrderNumber, account.Id,
                mockPaymentId);
            _context.Transactions.Add(newTransaction);
            
            await _publisher.Publish(integrationEvent with {Reason = "User has enough balance", IsValid = true});

            await _context.SaveChangesAsync(default);
        }
    }
}