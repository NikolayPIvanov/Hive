using System;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using BuildingBlocks.Core.Interfaces;
using DotNetCore.CAP;
using Hive.Billing.Contracts.IntegrationEvents;
using Hive.Billing.Domain.Entities;
using Hive.Billing.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Contracts.IntegrationEvents;

namespace Billing.Application.IntegrationEvents.EventHandlers
{
    public class OrderPlacedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IBillingDbContext _context;
        private readonly IIntegrationEventPublisher _publisher;
        private readonly ILogger<OrderPlacedIntegrationEventHandler> _logger;

        public OrderPlacedIntegrationEventHandler(
            IBillingDbContext context, IIntegrationEventPublisher publisher, ILogger<OrderPlacedIntegrationEventHandler> logger)
        {
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        [CapSubscribe(nameof(OrderPlacedIntegrationEvent), Group = "cap.hive.billing")]
        public async Task Handle(OrderPlacedIntegrationEvent @event)
        {
            var accountHolderId = @event.BuyerUserId;
            
            var wallet = await _context.Wallets
                .Include(w => w.AccountHolder)
                .FirstOrDefaultAsync(a => a.AccountHolder.UserId == accountHolderId);
            
            var integrationEvent = new BuyerBalanceVerifiedIntegrationEvent(@event.OrderNumber, "Account Holder does not exist", IsValid: false);

            if (wallet == null)
            {
                _logger.LogWarning("Account holder with {@UserId} has not been found.", @event.BuyerUserId);
                await _publisher.PublishAsync(integrationEvent);
                return;
            }
            //
            // if (wallet == null)
            // {
            //     _logger.LogWarning("Account holder with {@UserId} has not set up a default payment method.", @event.BuyerUserId);
            //     integrationEvent = integrationEvent with {Reason = "A default payment method has not been found."};
            //     await _publisher.PublishAsync(integrationEvent);
            //     return;
            // }
            
            if (wallet.Balance < @event.UnitPrice || wallet.Balance < 0.0m)
            {
                _logger.LogWarning("Account balance for {@AccountHolder} does not have enough funds {@Funds}", @event.BuyerUserId, wallet.Balance);
                integrationEvent = integrationEvent with {Reason = "User account does not have enough resources."};
                await _publisher.PublishAsync(integrationEvent);
                return;
            }
            
            var paymentTransaction = new Transaction(@event.UnitPrice, @event.OrderNumber, TransactionType.Payment);
            wallet.AddTransaction(paymentTransaction);

            await _context.SaveChangesAsync(default);
            
            await _publisher.PublishAsync(integrationEvent with {Reason = "User has enough balance", IsValid = true});
        }
    }
}