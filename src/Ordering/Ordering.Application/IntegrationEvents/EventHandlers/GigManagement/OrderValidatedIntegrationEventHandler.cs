﻿using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Gig.Contracts.IntegrationEvents;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Application.Orders.EventHandlers;
using Ordering.Domain.Enums;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.IntegrationEvents.EventHandlers.GigManagement
{
    public class OrderValidatedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IOrderingContext _context;
        private readonly IMediator _mediator;

        public OrderValidatedIntegrationEventHandler(IOrderingContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }
        
        [CapSubscribe(nameof(OrderValidatedIntegrationEvent), Group = "cap.hive.ordering")] 
        public async Task Handle(OrderValidatedIntegrationEvent @event)
        {
            var order = await _context.Orders
                .Select(o => new { o.Id, o.OrderNumber, o.UnitPrice, o.BuyerId, o.OrderStates})
                .FirstOrDefaultAsync(o => o.OrderNumber == @event.OrderNumber);

            var orderState = @event.IsValid ? OrderState.OrderDataValid : OrderState.Invalid;
            var state = new State(orderState, @event.Reason);

            var buyer = await _context.Buyers.FindAsync(order.BuyerId);

            if (orderState == OrderState.OrderDataValid)
            {
                await _mediator.Publish(new OrderValidatedEvent(@event.OrderNumber, order.UnitPrice, buyer.UserId));
            }

            order.OrderStates.Add(state);
            await _context.SaveChangesAsync(default);
        }
    }
}