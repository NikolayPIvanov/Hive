using Hive.Common.Core.SeedWork;

namespace Hive.Gig.Contracts.IntegrationEvents
{
    using System;
    using Common.Domain.SeedWork;
    
    public record OrderValidatedIntegrationEvent(Guid OrderNumber, string Reason, bool IsValid = true) :
        IntegrationEvent(nameof(OrderValidatedIntegrationEvent));
}