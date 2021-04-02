using Hive.Common.Domain.SeedWork;

namespace Hive.Identity.Contracts.IntegrationEvents
{
    public record UserCreatedIntegrationEvent(string UserId) : IntegrationEvent(nameof(UserCreatedIntegrationEvent));
}