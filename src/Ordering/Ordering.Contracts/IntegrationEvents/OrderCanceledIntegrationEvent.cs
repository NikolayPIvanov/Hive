using System;
using Hive.Common.Core.SeedWork;

namespace Ordering.Contracts.IntegrationEvents
{
    public record OrderCanceledIntegrationEvent(Guid OrderNumber, string CanceledBy) : IntegrationEvent(nameof(OrderCanceledIntegrationEvent));
    public record OrderDeclinedIntegrationEvent(Guid OrderNumber, string DeclinedBy) : IntegrationEvent(nameof(OrderDeclinedIntegrationEvent));
    public record OrderAcceptedIntegrationEvent(Guid OrderNumber, string AcceptedBy) : IntegrationEvent(nameof(OrderAcceptedIntegrationEvent));

}