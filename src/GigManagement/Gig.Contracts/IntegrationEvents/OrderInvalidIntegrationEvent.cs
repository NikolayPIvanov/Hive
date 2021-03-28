using System;
using Hive.Common.Domain;

namespace Hive.Gig.Domain.IntegrationEvents
{
    public record OrderInvalidIntegrationEvent(Guid OrderNumber, string Reason) : 
        IntegrationEvent(nameof(OrderInvalidIntegrationEvent));
    
    public record OrderBalanceInvalidIntegrationEvent(Guid OrderNumber, string Reason) : 
        IntegrationEvent(nameof(OrderBalanceInvalidIntegrationEvent));
}