using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Gig.Contracts.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;

namespace Ordering.Application.IntegrationEvents.EventHandlers.GigManagement
{
    public class OrderValidatedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IOrderingContext _context;

        public OrderValidatedIntegrationEventHandler(IOrderingContext context)
        {
            _context = context;
        }
        
        [CapSubscribe(nameof(OrderValidatedIntegrationEvent), Group = "cap.hive.ordering")] 
        public async Task Handle(OrderValidatedIntegrationEvent @event)
        {
            var order = await _context.Orders
                .Select(o => new { o.Id, o.OrderNumber })
                .FirstOrDefaultAsync(o => o.OrderNumber == @event.OrderNumber);

            var orderState = @event.IsValid ? OrderState.OrderValid : OrderState.Invalid;
            var state = new State(orderState, @event.Reason, order.Id);
            _context.OrderStates.Add(state);
            await _context.SaveChangesAsync(default);
        }
    }
}