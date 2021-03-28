using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Common.Application.Interfaces;
using Hive.Common.Domain;
using Ordering.Application.Interfaces;

namespace Ordering.Infrastructure.MessageBroker
{
    public class IntegrationEventPublisher : IIntegrationEventPublisher 
    {
        private readonly IOrderingContext _context;
        private readonly ICapPublisher _publisher;

        public IntegrationEventPublisher(IOrderingContext context, ICapPublisher publisher)
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