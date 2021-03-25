using System;
using Hive.Common.Domain;

namespace Gig.Contracts.IntegrationEvents
{
    public record OrderDataConfirmationIntegrationEvent(Guid OrderNumber, string Reason) :
        IntegrationEvent(nameof(OrderDataConfirmationIntegrationEvent));
}