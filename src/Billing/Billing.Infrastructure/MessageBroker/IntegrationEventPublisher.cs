using System.Threading.Tasks;
using Billing.Application.Interfaces;
using DotNetCore.CAP;
using Hive.Common.Application.Interfaces;
using Hive.Common.Domain;

namespace Billing.Infrastructure.MessageBroker
{
    public class IntegrationEventPublisher : IIntegrationEventPublisher 
    {
        private readonly IBillingContext _context;
        private readonly ICapPublisher _publisher;

        public IntegrationEventPublisher(IBillingContext context, ICapPublisher publisher)
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