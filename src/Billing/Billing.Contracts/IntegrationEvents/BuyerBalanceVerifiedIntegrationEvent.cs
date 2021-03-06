﻿using System;
using BuildingBlocks.Core.MessageBus;
using Hive.Common.Core.SeedWork;
using Hive.Common.Domain.SeedWork;

namespace Hive.Billing.Contracts.IntegrationEvents
{
    public record BuyerBalanceVerifiedIntegrationEvent(Guid OrderNumber, string Reason, bool IsValid = true) 
        : IntegrationEvent(nameof(BuyerBalanceVerifiedIntegrationEvent));
}