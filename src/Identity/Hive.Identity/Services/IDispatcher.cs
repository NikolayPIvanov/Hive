using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Application.Interfaces;
using Hive.Identity.Contracts.IntegrationEvents;

namespace Hive.Identity.Services
{
    // TODO: Can be a single method but will hide implementation
    public interface IDispatcher
    {
        Task PublishUserCreatedEventAsync(string userId,
            CancellationToken cancellationToken = new CancellationToken());
        Task PublishBuyerCreatedEventAsync(string userId,
            CancellationToken cancellationToken = new CancellationToken());
        
        Task PublishSellerCreatedEventAsync(string userId,
            CancellationToken cancellationToken = new CancellationToken());
        
        Task PublishInvestorCreatedEventAsync(string userId,
            CancellationToken cancellationToken = new CancellationToken());
    }
    
    public class EventDispatcher : IDispatcher
    {
        private readonly IIntegrationEventPublisher _publisher;

        public EventDispatcher(IIntegrationEventPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task PublishUserCreatedEventAsync(string userId, CancellationToken cancellationToken = new CancellationToken())
        {
            // Goes to User profile management
            var @event = new UserCreatedIntegrationEvent(userId);
            await _publisher.Publish(@event);
        }

        public async Task PublishBuyerCreatedEventAsync(string userId, CancellationToken cancellationToken = new CancellationToken())
        {
            // Goes into ordering, billing?
            var @event = new BuyerCreatedIntegrationEvent(userId);
            await _publisher.Publish(@event);
        }

        public async Task PublishSellerCreatedEventAsync(string userId, CancellationToken cancellationToken = new CancellationToken())
        {
            // Goes into gig management
            var @event = new SellerCreatedIntegrationEvent(userId);
            await _publisher.Publish(@event);
        }

        public async Task PublishInvestorCreatedEventAsync(string userId, CancellationToken cancellationToken = new CancellationToken())
        {
            // Goes into investing
            var @event = new InvestorCreatedIntegrationEvent(userId);
            await _publisher.Publish(@event);
        }
    }
}