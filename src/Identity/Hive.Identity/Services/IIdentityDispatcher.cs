using System;
using System.Threading.Tasks;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.SeedWork;
using Hive.Identity.Contracts;
using Hive.Identity.Contracts.IntegrationEvents;
using Hive.Identity.Models;

namespace Hive.Identity.Services
{
    public interface IIdentityDispatcher
    {
        Task PublishUserCreatedEventAsync(string userId);

        Task PublishUserTypeEventAsync(string userId, IdentityType accountType);
    }
    
    public class IdentityDispatcher : IIdentityDispatcher
    {
        private readonly IIntegrationEventPublisher _publisher;

        public IdentityDispatcher(IIntegrationEventPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task PublishUserCreatedEventAsync(string userId) =>
            await _publisher.Publish(new UserCreatedIntegrationEvent(userId));
        
        public async Task PublishUserTypeEventAsync(string userId, IdentityType accountType)
        {
            IntegrationEvent userTypeEvent =
                accountType switch
                {
                    IdentityType.Admin or IdentityType.Buyer => new BuyerCreatedIntegrationEvent(userId),
                    IdentityType.Seller => new SellerCreatedIntegrationEvent(userId),
                    IdentityType.Investor => new InvestorCreatedIntegrationEvent(userId),
                    _ => throw new ArgumentOutOfRangeException(nameof(accountType), accountType, null)
                };

            await _publisher.Publish(userTypeEvent);
        }
    }
}