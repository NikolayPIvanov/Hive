using System.Threading.Tasks;
using Gig.Contracts.IntegrationEvents;

namespace Hive.Gig.Application.Interfaces
{
    public interface IIntegrationEventPublisher
    {
        Task Publish<T>(T integrationEvent) where T : IntegrationEvent;
    }
}