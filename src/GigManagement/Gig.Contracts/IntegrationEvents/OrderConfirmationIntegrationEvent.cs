using System;
using Hive.Common.Domain;

namespace Hive.Gig.Contracts.IntegrationEvents
{
    public record OrderDataConfirmationIntegrationEvent(Guid OrderNumber, string Reason) :
        IntegrationEvent(nameof(OrderDataConfirmationIntegrationEvent));
}