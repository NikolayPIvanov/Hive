using System;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using BuildingBlocks.Core.Interfaces;
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
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        [CapSubscribe(nameof(OrderPlacedIntegrationEvent), Group = "cap.hive.billing")]
        public async Task Handle(OrderPlacedIntegrationEvent @event)
        {
            var accountHolderId = @event.BuyerUserId;
            
            var account = await _context.AccountHolders
                .Include(w => w.Wallet)
                .FirstOrDefaultAsync(a => a.UserId == accountHolderId);
            
            var integrationEvent = new BuyerBalanceVerifiedIntegrationEvent(@event.OrderNumber, "Account Holder does not exist", IsValid: false);

            if (account == null)
            {
                _logger.LogWarning("Account holder with {@UserId} has not been found.", @event.BuyerUserId);
                await _publisher.PublishAsync(integrationEvent);
                return;
            }
            
            if (account.Wallet == null)
            {
                _logger.LogWarning("Account holder with {@UserId} has not set up a default payment method.", @event.BuyerUserId);
                integrationEvent = integrationEvent with {Reason = "A default payment method has not been found."};
                await _publisher.PublishAsync(integrationEvent);
                return;
            }
            
            if (account.Wallet.Balance < @event.UnitPrice || account.Wallet.Balance < 0.0m)
            {
                _logger.LogWarning("Account balance for {@AccountHolder} does not have enough funds {@Funds}", @event.BuyerUserId, account.Wallet.Balance);
                integrationEvent = integrationEvent with {Reason = "User account does not have enough resources."};
                await _publisher.PublishAsync(integrationEvent);
                return;
            }
            
            var paymentTransaction = new Transaction(@event.UnitPrice, @event.OrderNumber, TransactionType.Payment);
            account.Wallet.AddTransaction(paymentTransaction);

            await _context.SaveChangesAsync(default);
            
            await _publisher.PublishAsync(integrationEvent with {Reason = "User has enough balance", IsValid = true});
        }
    }
}