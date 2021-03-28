using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Common.Application.Interfaces;
using Hive.Common.Domain;

namespace Common.Infrastructure.Services
{
    public class IntegrationEventPublisher : IIntegrationEventPublisher
    {
        private readonly ICapPublisher _publisher;

        public IntegrationEventPublisher(ICapPublisher publisher)
        {
            _publisher = publisher;
        }
        
        public async Task Publish<T>(T integrationEvent) where T : IntegrationEvent
        {
            await _publisher.PublishAsync(integrationEvent.Name, integrationEvent);
        }
    }
}