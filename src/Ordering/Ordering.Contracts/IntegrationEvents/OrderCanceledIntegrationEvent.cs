using System;
using Hive.Common.Domain.SeedWork;

namespace Ordering.Contracts.IntegrationEvents
{
    public record OrderCanceledIntegrationEvent(Guid OrderNumber, string CanceledBy) : IntegrationEvent(nameof(OrderCanceledIntegrationEvent));
    public record OrderDeclinedIntegrationEvent(Guid OrderNumber, string DeclinedBy) : IntegrationEvent(nameof(OrderCanceledIntegrationEvent));

}