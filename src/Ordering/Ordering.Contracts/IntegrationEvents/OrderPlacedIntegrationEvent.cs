using System;
using Hive.Common.Domain.SeedWork;

namespace Ordering.Contracts.IntegrationEvents
{
    public record OrderPlacedIntegrationEvent(Guid OrderNumber, decimal UnitPrice,
            string UserId, int SellerId, int GigId, int PackageId)
        : IntegrationEvent(nameof(OrderPlacedIntegrationEvent));
    
    public record OrderBuyerVerificationIntegrationEvent(Guid OrderNumber, decimal UnitPrice, string UserId)
        : IntegrationEvent(nameof(OrderBuyerVerificationIntegrationEvent));
}