using System;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using BuildingBlocks.Core.MessageBus;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.SeedWork;
using Hive.Identity.Contracts;
using Hive.Identity.Contracts.IntegrationEvents;
using Hive.Identity.Contracts.IntegrationEvents.Outbound;
using Hive.Identity.Models;
using Microsoft.Extensions.Logging;

namespace Hive.Identity.Services
{
    public interface IIdentityDispatcher
    {
        Task PublishUserCreatedEventAsync(string userId, string givenName, string surname);

        Task PublishUserTypeEventAsync(string userId, IdentityType accountType);
    }
    
    public class IdentityDispatcher : IIdentityDispatcher
    {
        private readonly IIntegrationEventPublisher _publisher;
        private readonly ILogger<IdentityDispatcher> _logger;

        public IdentityDispatcher(IIntegrationEventPublisher publisher, ILogger<IdentityDispatcher> logger)
        {
            _publisher = publisher;
            _logger = logger;
        }

        public async Task PublishUserCreatedEventAsync(string userId, string givenName, string surname)
        {
            _logger.LogInformation($"Event User for {userId}");
            await _publisher.PublishAsync(new UserCreatedIntegrationEvent(userId, givenName, surname));
        }
        
        public async Task PublishUserTypeEventAsync(string userId, IdentityType accountType)
        {
            _logger.LogInformation($"Event for user type for {userId}");

            IntegrationEvent userTypeEvent =
                accountType switch
                {
                    IdentityType.Admin or IdentityType.Buyer => new BuyerCreatedIntegrationEvent(userId),
                    IdentityType.Seller => new SellerCreatedIntegrationEvent(userId),
                    IdentityType.Investor => new InvestorCreatedIntegrationEvent(userId),
                    _ => throw new ArgumentOutOfRangeException(nameof(accountType), accountType, null)
                };

            await _publisher.PublishAsync(userTypeEvent);
        }
    }
}