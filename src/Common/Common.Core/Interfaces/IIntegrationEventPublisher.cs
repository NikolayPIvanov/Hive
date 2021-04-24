using System.Threading.Tasks;
using Hive.Common.Core.SeedWork;
using Hive.Common.Domain.SeedWork;

namespace Hive.Common.Core.Interfaces
{
    public interface IIntegrationEventPublisher
    {
        Task Publish<T>(T integrationEvent) where T : IntegrationEvent;
    }
}