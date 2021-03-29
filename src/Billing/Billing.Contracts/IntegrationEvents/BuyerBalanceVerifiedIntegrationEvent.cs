using System;
using Hive.Common.Domain;

namespace Hive.Billing.Contracts.IntegrationEvents
{
    public record BuyerBalanceVerifiedIntegrationEvent(Guid OrderNumber, string Reason, bool IsValid = true) 
        : IntegrationEvent(nameof(BuyerBalanceVerifiedIntegrationEvent));
}