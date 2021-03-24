using System;
using Hive.Common.Domain;

namespace Gig.Contracts.IntegrationEvents
{
    public record OrderConfirmationIntegrationEvent(Guid OrderNumber) :
        IntegrationEvent(nameof(OrderConfirmationIntegrationEvent));
}