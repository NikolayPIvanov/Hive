using Hive.Common.Domain;

namespace Hive.Contracts.IntegrationEvents
{
    public record SellerCreatedIntegrationEvent(string UserId) : IntegrationEvent(nameof(SellerCreatedIntegrationEvent));
}