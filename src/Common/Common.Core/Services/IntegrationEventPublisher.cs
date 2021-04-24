using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.SeedWork;
using Hive.Common.Domain.SeedWork;

namespace Hive.Common.Core.Services
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
            await _publisher.PublishAsync(integrationEvent.Name, integrationEvent, integrationEvent.Callback);
        }
    }
}