﻿using System;
using BuildingBlocks.Core.MessageBus;
using Hive.Common.Core.SeedWork;
using Hive.Common.Domain.SeedWork;

namespace Ordering.Contracts.IntegrationEvents
{
    public record OrderPlacedIntegrationEvent(Guid OrderNumber, decimal UnitPrice,
            string BuyerUserId, string SellerUserId, int PackageId)
        : IntegrationEvent(nameof(OrderPlacedIntegrationEvent));
    
    public record OrderBuyerVerificationIntegrationEvent(
        Guid OrderNumber, decimal UnitPrice, string UserId, int PackageId, int GigId)
        : IntegrationEvent(nameof(OrderBuyerVerificationIntegrationEvent));
}