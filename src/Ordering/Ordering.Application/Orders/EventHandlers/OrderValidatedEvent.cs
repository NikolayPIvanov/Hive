﻿using System;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using Hive.Common.Core.Interfaces;
using MediatR;
using Ordering.Contracts.IntegrationEvents;

namespace Ordering.Application.Orders.EventHandlers
{
    public record OrderValidatedEvent(
        Guid OrderNumber, decimal UnitPrice, string UserId, int PackageId, int GigId) : INotification;

    public class OrderValidatedEventHandler : INotificationHandler<OrderValidatedEvent>
    {
        private readonly IIntegrationEventPublisher _publisher;

        public OrderValidatedEventHandler(IIntegrationEventPublisher publisher)
        {
            _publisher = publisher;
        }
        
        public async Task Handle(OrderValidatedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new OrderBuyerVerificationIntegrationEvent(
                notification.OrderNumber, 
                notification.UnitPrice,
                notification.UserId,
                notification.PackageId,
                notification.GigId);
            await _publisher.PublishAsync(@event);
        }
    }
}