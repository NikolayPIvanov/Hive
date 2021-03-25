using System;
using Hive.Common.Domain;

namespace Hive.Billing.Contracts.IntegrationEvents
{
    public record OrderBalanceConfirmationIntegrationEvent(Guid OrderNumber, string Reason) 
        : IntegrationEvent(nameof(OrderBalanceConfirmationIntegrationEvent));
}