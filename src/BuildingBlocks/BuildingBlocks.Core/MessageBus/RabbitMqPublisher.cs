using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using DotNetCore.CAP;

namespace BuildingBlocks.Core.MessageBus
{
    public class RabbitMqPublisher : IIntegrationEventPublisher
    {
        private readonly ICapPublisher _publisher;

        public RabbitMqPublisher(ICapPublisher publisher)
        {
            _publisher = publisher;
        }
        
        public async Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken) where T : IntegrationEvent
        {
            await _publisher.PublishAsync(integrationEvent.Name, integrationEvent, integrationEvent.Callback, cancellationToken);
        }
    }
}