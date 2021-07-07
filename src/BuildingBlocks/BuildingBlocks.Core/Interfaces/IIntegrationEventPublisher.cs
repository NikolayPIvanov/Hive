using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.MessageBus;

namespace BuildingBlocks.Core.Interfaces
{
    public interface IIntegrationEventPublisher
    {
        Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) where T : IntegrationEvent;
    }
}