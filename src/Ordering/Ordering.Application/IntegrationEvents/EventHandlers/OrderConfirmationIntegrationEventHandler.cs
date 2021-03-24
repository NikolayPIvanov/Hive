using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Gig.Contracts.IntegrationEvents;
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
        
        [CapSubscribe(nameof(OrderConfirmationIntegrationEvent))] 
        public async Task Handle(OrderConfirmationIntegrationEvent @event)
        {
            var orderStatus = await _context.Orders
                .Select(o => new
                {
                    o.OrderNumber,
                    o.OrderStates
                })
                .FirstOrDefaultAsync(o => o.OrderNumber == @event.OrderNumber);

            var reason = "Waiting for seller to review order";
            var state = new State(OrderState.Pending, reason);
            orderStatus.OrderStates.Add(state);
            
            await _context.SaveChangesAsync(new CancellationToken());
        }
    }
}