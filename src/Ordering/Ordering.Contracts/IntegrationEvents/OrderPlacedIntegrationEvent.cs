using System;
using Hive.Common.Domain;

namespace Ordering.Contracts.IntegrationEvents
{
    public record OrderPlacedIntegrationEvent(Guid OrderNumber, decimal UnitPrice,
            string UserId, int SellerId, int GigId, int PackageId)
        : IntegrationEvent(nameof(OrderPlacedIntegrationEvent));
    
    public record OrderBuyerVerificationIntegrationEvent(Guid OrderNumber, decimal UnitPrice, string UserId)
        : IntegrationEvent(nameof(OrderBuyerVerificationIntegrationEvent));
}