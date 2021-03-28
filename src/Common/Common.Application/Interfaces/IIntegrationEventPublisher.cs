using System.Threading.Tasks;
using Hive.Common.Domain;

namespace Hive.Common.Application.Interfaces
{
    public interface IIntegrationEventPublisher
    {
        Task Publish<T>(T integrationEvent) where T : IntegrationEvent;
    }
}