using System;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.MessageBus;
using DotNetCore.CAP;
using Hive.Common.Core.SeedWork;
using MediatR;
using Ordering.Contracts.IntegrationEvents;
using Ordering.Domain.Enums;

namespace Ordering.Application.Orders.EventHandlers
{
    public record OrderReviewedEvent(Guid OrderNumber, OrderState OrderState, string PerformedBy) : INotification;

    public class OrderReviewedEventHandler : INotificationHandler<OrderReviewedEvent>
    {
        private readonly ICapPublisher _publisher;

        public OrderReviewedEventHandler(ICapPublisher publisher)
        {
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        }
        
        public async Task Handle(OrderReviewedEvent notification, CancellationToken cancellationToken)
        {
            IntegrationEvent integrationEvent =
                notification.OrderState switch
                {
                    OrderState.Declined => new OrderDeclinedIntegrationEvent(notification.OrderNumber,
                        notification.PerformedBy),
                    _ => throw new NotSupportedException()
                };
            await _publisher.PublishAsync(integrationEvent.Name, integrationEvent, cancellationToken: cancellationToken);
            // TODO: Send email to the seller that a user has canceled an order that was yet not reviewed
        }
    }
}