using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using DotNetCore.CAP;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Core.MessageBus
{
    public class RabbitMqPublisher : IIntegrationEventPublisher
    {
        private readonly ICapPublisher _publisher;
        private readonly ILogger<RabbitMqPublisher> _logger;

        public RabbitMqPublisher(ICapPublisher publisher, ILogger<RabbitMqPublisher> logger)
        {
            _publisher = publisher;
            _logger = logger;
        }
        
        public async Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken) where T : IntegrationEvent
        {
            _logger.LogInformation($"Started publishing event of type {typeof(T)}");
            await _publisher.PublishAsync(integrationEvent.Name, integrationEvent, integrationEvent.Callback, cancellationToken);
            _logger.LogInformation($"Published event of type {typeof(T)}");

        }
    }
}