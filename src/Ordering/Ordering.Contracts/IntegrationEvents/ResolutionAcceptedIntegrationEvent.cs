using BuildingBlocks.Core.MessageBus;

namespace Ordering.Contracts.IntegrationEvents
{
    // To investing to get ROI + investors
    public record ResolutionAcceptedIntegrationEvent(int GigId, int ResolutionId) : IntegrationEvent(nameof(ResolutionAcceptedIntegrationEvent));
}