using System.Threading.Tasks;
using DotNetCore.CAP;
using Gig.Contracts.IntegrationEvents;
using Hive.Gig.Application.Interfaces;

namespace Hive.Gig.Infrastructure.MessageBroker
{
    public class IntegrationEventPublisher : IIntegrationEventPublisher 
    {
        private readonly IGigManagementContext _context;
        private readonly ICapPublisher _publisher;

        public IntegrationEventPublisher(IGigManagementContext context, ICapPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }
        
        public async Task Publish<T>(T integrationEvent) where T : IntegrationEvent
        {
            await _publisher.PublishAsync(integrationEvent.Name, integrationEvent);
        }
    }
}