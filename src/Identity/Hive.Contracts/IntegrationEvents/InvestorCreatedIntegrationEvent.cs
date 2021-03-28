using Hive.Common.Domain;

namespace Hive.Contracts.IntegrationEvents
{
    public record InvestorCreatedIntegrationEvent(string UserId) : IntegrationEvent(nameof(InvestorCreatedIntegrationEvent));
}