using Hive.Common.Domain;
using Hive.Common.Domain.SeedWork;

namespace Hive.Identity.Contracts.IntegrationEvents
{
    public record BuyerCreatedIntegrationEvent(string UserId) : IntegrationEvent(nameof(BuyerCreatedIntegrationEvent));
}