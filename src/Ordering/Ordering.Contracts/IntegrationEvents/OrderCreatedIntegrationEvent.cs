using System;
using Hive.Common.Domain;

namespace Ordering.Contracts.IntegrationEvents
{
    public record OrderCreatedIntegrationEvent(Guid OrderNumber, decimal UnitPrice,
            string OrderedBy, int SellerId, int GigId, int PackageId)
        : IntegrationEvent(nameof(OrderCreatedIntegrationEvent));
}