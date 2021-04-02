using System.Threading.Tasks;
using Hive.Common.Domain;
using Hive.Common.Domain.SeedWork;

namespace Hive.Common.Application.Interfaces
{
    public interface IIntegrationEventPublisher
    {
        Task Publish<T>(T integrationEvent) where T : IntegrationEvent;
    }
}