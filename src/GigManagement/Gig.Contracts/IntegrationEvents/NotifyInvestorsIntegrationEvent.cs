using BuildingBlocks.Core.MessageBus;

namespace Hive.Gig.Contracts.IntegrationEvents
{
    public record NotifyInvestorsIntegrationEvent(int GigId, int PlanId) : IntegrationEvent(nameof(NotifyInvestorsIntegrationEvent));
}