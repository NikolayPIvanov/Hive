using System.Threading.Tasks;
using Hive.Common.Core.Interfaces;
using Hive.Common.Domain.SeedWork;

namespace Hive.Identity.Services
{
    public class IntegrationEventPublisher : IIntegrationEventPublisher
    {
        public Task Publish<T>(T integrationEvent) where T : IntegrationEvent
        {
            throw new System.NotImplementedException();
        }
    }
}