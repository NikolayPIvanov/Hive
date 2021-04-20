using Hive.Common.Domain.SeedWork;

namespace Hive.Identity.Contracts.IntegrationEvents
{
    public record SellerCreatedIntegrationEvent(string UserId) : IntegrationEvent(nameof(SellerCreatedIntegrationEvent));
}