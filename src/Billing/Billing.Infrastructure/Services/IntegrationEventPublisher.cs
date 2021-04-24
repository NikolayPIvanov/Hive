using System.Threading.Tasks;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.SeedWork;
using Hive.Common.Domain.SeedWork;

namespace Billing.Infrastructure.Services
{
    public class IntegrationEventPublisher : IIntegrationEventPublisher
    {
        public Task Publish<T>(T integrationEvent) where T : IntegrationEvent
        {
            throw new System.NotImplementedException();
        }
    }
}