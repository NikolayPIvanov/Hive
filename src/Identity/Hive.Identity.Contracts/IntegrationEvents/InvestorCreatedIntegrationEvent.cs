using Hive.Common.Domain;
using Hive.Common.Domain.SeedWork;

namespace Hive.Identity.Contracts.IntegrationEvents
{
    public record InvestorCreatedIntegrationEvent(string UserId) : IntegrationEvent(nameof(InvestorCreatedIntegrationEvent));
}