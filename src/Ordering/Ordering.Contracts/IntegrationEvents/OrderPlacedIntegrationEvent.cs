using System;
using Hive.Common.Core.SeedWork;
using Hive.Common.Domain.SeedWork;

namespace Ordering.Contracts.IntegrationEvents
{
    public record OrderPlacedIntegrationEvent(Guid OrderNumber, decimal UnitPrice,
            string BuyerUserId, string SellerUserId, int PackageId)
        : IntegrationEvent(nameof(OrderPlacedIntegrationEvent));
    
    public record OrderBuyerVerificationIntegrationEvent(Guid OrderNumber, decimal UnitPrice, string UserId)
        : IntegrationEvent(nameof(OrderBuyerVerificationIntegrationEvent));
}