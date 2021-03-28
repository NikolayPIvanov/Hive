using Hive.Common.Domain;

namespace Hive.Identity.Contracts.IntegrationEvents
{
    public record UserCreatedIntegrationEvent(string UserId) : IntegrationEvent(nameof(UserCreatedIntegrationEvent));
}