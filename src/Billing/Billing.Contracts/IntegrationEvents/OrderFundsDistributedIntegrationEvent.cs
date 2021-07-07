using System;
using BuildingBlocks.Core.MessageBus;

namespace Hive.Billing.Contracts.IntegrationEvents
{
    public record OrderFundsDistributedIntegrationEvent(Guid OrderNumber, int ResolutionId) : IntegrationEvent(
        nameof(OrderFundsDistributedIntegrationEvent));
}