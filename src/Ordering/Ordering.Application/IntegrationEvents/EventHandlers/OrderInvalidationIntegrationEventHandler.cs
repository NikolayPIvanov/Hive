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
    public class OrderInvalidationIntegrationEventHandler : ICapSubscribe
    {
        private readonly IOrderingContext _context;

        public OrderInvalidationIntegrationEventHandler(IOrderingContext context)
        {
            _context = context;
        }
        
        [CapSubscribe(nameof(OrderInvalidIntegrationEvent))] 
        public async Task Handle(OrderInvalidIntegrationEvent @event)
        {
            var orderStatus = await _context.Orders
                .Select(o => new
                {
                    o.OrderNumber,
                    o.OrderStates
                })
                .FirstOrDefaultAsync(o => o.OrderNumber == @event.OrderNumber);

            var state = new State(OrderState.Invalid, @event.Reason);
            orderStatus.OrderStates.Add(state);

            await _context.SaveChangesAsync(new CancellationToken());
        }
    }
}