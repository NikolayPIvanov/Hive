using System;
using System.Threading.Tasks;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.SeedWork;
using Hive.Common.Domain.SeedWork;
using Hive.Identity.Contracts.IntegrationEvents;
using Hive.Identity.Models;

namespace Hive.Identity.Services
{
    public interface IIdentityDispatcher
    {
        Task PublishUserCreatedEventAsync(string userId, AccountType accountType);
    }
    
    public class IdentityDispatcher : IIdentityDispatcher
    {
        private readonly IIntegrationEventPublisher _publisher;

        public IdentityDispatcher(IIntegrationEventPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task PublishUserCreatedEventAsync(string userId, AccountType accountType)
        {
            var @event = new UserCreatedIntegrationEvent(userId);
            var userTask = _publisher.Publish(@event);

            IntegrationEvent userTypeEvent =
                accountType switch
                {
                    AccountType.Admin or AccountType.Buyer => new BuyerCreatedIntegrationEvent(userId),
                    AccountType.Seller => new SellerCreatedIntegrationEvent(userId),
                    AccountType.Investor => new InvestorCreatedIntegrationEvent(userId),
                    _ => throw new ArgumentOutOfRangeException(nameof(accountType), accountType, null)
                };

            await Task.WhenAll(_publisher.Publish(userTypeEvent), userTask);
        }
    }
}