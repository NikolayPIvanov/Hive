using System;
using BuildingBlocks.Core.MessageBus;

namespace Ordering.Contracts.IntegrationEvents
{
    public record OrderCompletedIntegrationEvent(Guid OrderNumber, decimal BasePrice, decimal Tax, string BuyerUserId, string SellerUserId)
        : IntegrationEvent(nameof(OrderCompletedIntegrationEvent));
}