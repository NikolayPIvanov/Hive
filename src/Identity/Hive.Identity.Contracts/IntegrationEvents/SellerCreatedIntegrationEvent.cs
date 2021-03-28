using Hive.Common.Domain;

namespace Hive.Identity.Contracts.IntegrationEvents
{
    public record SellerCreatedIntegrationEvent(string UserId) : IntegrationEvent(nameof(SellerCreatedIntegrationEvent));
}