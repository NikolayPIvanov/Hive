using Hive.Common.Domain;

namespace Hive.Identity.Contracts.IntegrationEvents
{
    public record BuyerCreatedIntegrationEvent(string UserId) : IntegrationEvent(nameof(BuyerCreatedIntegrationEvent));
}