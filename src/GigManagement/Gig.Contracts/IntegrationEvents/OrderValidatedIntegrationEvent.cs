using BuildingBlocks.Core.MessageBus;
using System;

namespace Hive.Gig.Contracts.IntegrationEvents
{
    public record OrderValidatedIntegrationEvent(
        Guid OrderNumber, int PackageId, int GigId, string Reason, bool IsValid = true) :
        IntegrationEvent(nameof(OrderValidatedIntegrationEvent));
}