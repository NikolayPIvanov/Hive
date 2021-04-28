using BuildingBlocks.Core.MessageBus;

namespace Hive.Gig.Contracts.IntegrationEvents
{
    using System;
    
    public record OrderValidatedIntegrationEvent(Guid OrderNumber, string Reason, bool IsValid = true) :
        IntegrationEvent(nameof(OrderValidatedIntegrationEvent));
}