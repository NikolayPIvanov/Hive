using BuildingBlocks.Core.MessageBus;

namespace Hive.Identity.Contracts.IntegrationEvents.Inbound
{
    public record ExternalAccountSetIntegrationEvent
        (string UserId, int ExternalAccountId) : IntegrationEvent(nameof(ExternalAccountSetIntegrationEvent));
}