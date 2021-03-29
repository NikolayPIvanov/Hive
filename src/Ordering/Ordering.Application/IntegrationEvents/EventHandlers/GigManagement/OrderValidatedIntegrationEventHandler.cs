using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Gig.Contracts.IntegrationEvents;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Application.Orders.EventHandlers;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;

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
                .Select(o => new { o.Id, o.OrderNumber, o.UnitPrice, UserId = o.Buyer.UserId})
                .FirstOrDefaultAsync(o => o.OrderNumber == @event.OrderNumber);

            var orderState = @event.IsValid ? OrderState.OrderValid : OrderState.Invalid;
            var state = new State(orderState, @event.Reason, order.Id);

            if (orderState == OrderState.OrderValid)
            {
                await _mediator.Publish(new OrderValidatedEvent(@event.OrderNumber, order.UnitPrice, order.UserId));
            }

            _context.OrderStates.Add(state);
            await _context.SaveChangesAsync(default);
        }
    }
}