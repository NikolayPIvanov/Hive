using System;
using Hive.Common.Domain;
using Hive.Common.Domain.SeedWork;

namespace Hive.Gig.Contracts.IntegrationEvents
{
    public record OrderValidatedIntegrationEvent(Guid OrderNumber, string Reason, bool IsValid = true) :
        IntegrationEvent(nameof(OrderValidatedIntegrationEvent));
}