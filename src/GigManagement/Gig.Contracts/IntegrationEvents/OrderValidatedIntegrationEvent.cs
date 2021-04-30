using BuildingBlocks.Core.MessageBus;
using System;

namespace Hive.Gig.Contracts.IntegrationEvents
{
    public record OrderValidatedIntegrationEvent(Guid OrderNumber, string Reason, bool IsValid = true) :
        IntegrationEvent(nameof(OrderValidatedIntegrationEvent));
}