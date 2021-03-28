using Hive.Common.Domain;

namespace Hive.Identity.Contracts.IntegrationEvents
{
    public record InvestorCreatedIntegrationEvent(string UserId) : IntegrationEvent(nameof(InvestorCreatedIntegrationEvent));
}