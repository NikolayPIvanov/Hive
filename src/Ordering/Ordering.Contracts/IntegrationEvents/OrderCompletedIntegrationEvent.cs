using System;
using System.Collections.Generic;
using BuildingBlocks.Core.MessageBus;

namespace Ordering.Contracts.IntegrationEvents
{
    public record WalletDataDeposit(string UserId, decimal Amount, string OrderNumber);
    
    public record OrderCompletedIntegrationEvent(
            Guid OrderNumber,
            int ResolutionId,
        string BuyerUserId, 
        string SellerUserId,
        decimal BasePrice, 
        decimal Tax,
        List<WalletDataDeposit> Data
        )
        : IntegrationEvent(nameof(OrderCompletedIntegrationEvent));
}