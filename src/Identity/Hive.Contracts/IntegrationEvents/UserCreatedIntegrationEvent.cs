using Hive.Common.Domain;

namespace Hive.Contracts.IntegrationEvents
{
    public record UserCreatedIntegrationEvent(string UserId) : IntegrationEvent(nameof(UserCreatedIntegrationEvent));
}