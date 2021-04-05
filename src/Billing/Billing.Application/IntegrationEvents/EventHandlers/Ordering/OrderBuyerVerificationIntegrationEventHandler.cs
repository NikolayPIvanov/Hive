using System;
using System.Linq;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using DotNetCore.CAP;
using Hive.Billing.Contracts.IntegrationEvents;
using Hive.Billing.Domain.Entities;
using Hive.Billing.Domain.Enums;
using Hive.Common.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Contracts.IntegrationEvents;

namespace Billing.Application.IntegrationEvents.EventHandlers.Ordering
{
    public class OrderBuyerVerificationIntegrationEventHandler : ICapSubscribe
    {
        private readonly IBillingDbContext _context;
        private readonly IIntegrationEventPublisher _publisher;
        private readonly ILogger<OrderBuyerVerificationIntegrationEventHandler> _logger;

        public OrderBuyerVerificationIntegrationEventHandler(
            IBillingDbContext context, IIntegrationEventPublisher publisher, ILogger<OrderBuyerVerificationIntegrationEventHandler> logger)
        {
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        [CapSubscribe(nameof(OrderPlacedIntegrationEvent), Group = "cap.hive.billing")]
        public async Task Handle(OrderPlacedIntegrationEvent @event)
        {
            var accountHolderId = @event.BuyerUserId;
            
            // TODO: Need to implement uplock
            var account = await _context.AccountHolders
                .Select(ah => new
                {
                    ah.UserId,
                    ah.Account.Id,
                    ah.Account.DefaultPaymentMethod,
                })
                .FirstOrDefaultAsync(a => a.UserId == accountHolderId);
            
            var integrationEvent = new BuyerBalanceVerifiedIntegrationEvent(@event.OrderNumber, "Account Holder does not exist", IsValid: false);

            if (account == null)
            {
                _logger.LogWarning("Account holder with {@UserId} has not been found.", accountHolderId);
                await _publisher.Publish(integrationEvent);
                return;
            }
            
            if (account.DefaultPaymentMethod == null)
            {
                _logger.LogWarning("Account holder with {@UserId} has not set up a default payment method.", accountHolderId);
                integrationEvent = integrationEvent with {Reason = "A default payment method has not been found."};
                await _publisher.Publish(integrationEvent);
                return;
            }

            var transactions =
                await _context.Transactions.Where(t => t.PaymentMethodId == account.DefaultPaymentMethod.Id)
                    .ToListAsync();

            // TODO: This needs to go into a snapshot
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
            
            if (balance < @event.UnitPrice || balance < 0.0m)
            {
                _logger.LogWarning("Account balance for {@AccountHolder} does not have enough funds {@Funds}", accountHolderId, balance);
                integrationEvent = integrationEvent with {Reason = "User account does not have enough resources."};
                await _publisher.Publish(integrationEvent);
                return;
            }
            //
            // var holdTransaction = new Transaction(TransactionType.Hold, @event.UnitPrice, account.DefaultPaymentMethod.Id, @event.OrderNumber);
            // _context.Transactions.Add(holdTransaction);
            //
            // await _context.SaveChangesAsync(default);
            //
            await _publisher.Publish(integrationEvent with {Reason = "User has enough balance", IsValid = true});
        }
    }
}