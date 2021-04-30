using BuildingBlocks.Core.MessageBus;

namespace Hive.Gig.Contracts.IntegrationEvents
{
    public record PlanGigCreatedIntegrationEvent(int GigId, int PlanId) : IntegrationEvent(nameof(PlanGigCreatedIntegrationEvent));
}