using Hive.Common.Domain;

namespace Hive.Contracts.IntegrationEvents
{
    public record BuyerCreatedIntegrationEvent(string UserId) : IntegrationEvent(nameof(BuyerCreatedIntegrationEvent));
}