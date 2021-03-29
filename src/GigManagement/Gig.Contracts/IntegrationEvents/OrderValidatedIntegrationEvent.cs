using System;
using Hive.Common.Domain;

namespace Hive.Gig.Contracts.IntegrationEvents
{
    public record OrderValidatedIntegrationEvent(Guid OrderNumber, string Reason, bool IsValid = true) :
        IntegrationEvent(nameof(OrderValidatedIntegrationEvent));
}