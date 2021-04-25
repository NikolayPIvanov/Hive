using System;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;
using MediatR;
using Ordering.Contracts.IntegrationEvents;

namespace Ordering.Application.Orders.EventHandlers
{
    public record OrderCanceledEvent(Guid OrderNumber, string CanceledBy) : INotification;

    public class OrderCanceledEventHandler : INotificationHandler<OrderCanceledEvent>
    {
        private readonly ICapPublisher _publisher;

        public OrderCanceledEventHandler(ICapPublisher publisher)
        {
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        }
        
        public async Task Handle(OrderCanceledEvent notification, CancellationToken cancellationToken)
        {
            var orderCanceledEvent = new OrderCanceledIntegrationEvent(notification.OrderNumber, notification.CanceledBy);
            await _publisher.PublishAsync(orderCanceledEvent.Name, orderCanceledEvent, cancellationToken: cancellationToken);
            // TODO: Send email to the seller that a user has canceled an order that was yet not reviewed
        }
    }
}