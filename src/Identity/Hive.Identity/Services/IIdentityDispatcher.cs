using System;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using BuildingBlocks.Core.MessageBus;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.SeedWork;
using Hive.Identity.Contracts;
using Hive.Identity.Contracts.IntegrationEvents;
using Hive.Identity.Models;

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

        public IdentityDispatcher(IIntegrationEventPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task PublishUserCreatedEventAsync(string userId, string givenName, string surname) =>
            await _publisher.PublishAsync(new UserCreatedIntegrationEvent(userId, givenName, surname));
        
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

            await _publisher.PublishAsync(userTypeEvent);
        }
    }
}