using System;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Contracts.IntegrationEvents;
using Ordering.Domain.Entities;

namespace Ordering.Application.Orders.EventHandlers
{
    public record OrderPlacedEvent(Order Order, string BuyerUserId) : INotification;
    
    public class OrderPlacedEventHandler : INotificationHandler<OrderPlacedEvent>
    {
        private readonly ICapPublisher _publisher;
        private readonly ILogger<OrderPlacedEventHandler> _logger;

        public OrderPlacedEventHandler(ICapPublisher publisher, ILogger<OrderPlacedEventHandler> logger)
        {
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task Handle(OrderPlacedEvent notification, CancellationToken cancellationToken)
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));
            
            _logger.LogInformation("Order Placed Domain event started for order: {@OrderNumber}", notification.Order.OrderNumber);
            var order = notification.Order;
            var orderCreated = new OrderPlacedIntegrationEvent(order.OrderNumber, order.UnitPrice, notification.BuyerUserId,
                order.SellerUserId, order.PackageId);

            await _publisher.PublishAsync(orderCreated.Name, orderCreated, cancellationToken: cancellationToken);
            
            // TODO: Send email or notification to seller?
            _logger.LogInformation("Order Placed Domain event finished for order: {@OrderNumber}", notification.Order.OrderNumber);
        }
    }
}