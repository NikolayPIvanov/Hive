using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Gig.Domain.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;

namespace Ordering.Application.IntegrationEvents.EventHandlers
{
    public class OrderConfirmationIntegrationEventHandler : ICapSubscribe
    {
        private readonly IOrderingContext _context;

        public OrderConfirmationIntegrationEventHandler(IOrderingContext context)
        {
            _context = context;
        }
        
        [CapSubscribe(nameof(OrderDataConfirmationIntegrationEvent), Group = "cap.hive.ordering")] 
        public async Task Handle(OrderDataConfirmationIntegrationEvent @event)
        {
            var orderStatus = await _context.Orders
                .Select(o => new { o.Id, o.OrderNumber })
                .FirstOrDefaultAsync(o => o.OrderNumber == @event.OrderNumber);

            var state = new State(OrderState.OrderValid, @event.Reason, orderStatus.Id);
            _context.OrderStates.Add(state);

            await _context.SaveChangesAsync(default);
        }
    }
}