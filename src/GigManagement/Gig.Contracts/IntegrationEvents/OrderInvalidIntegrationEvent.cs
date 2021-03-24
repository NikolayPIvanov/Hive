using System;
using Hive.Common.Domain;

namespace Gig.Contracts.IntegrationEvents
{
    public record OrderInvalidIntegrationEvent(Guid OrderNumber, string Reason) : 
        IntegrationEvent(nameof(OrderInvalidIntegrationEvent));
}