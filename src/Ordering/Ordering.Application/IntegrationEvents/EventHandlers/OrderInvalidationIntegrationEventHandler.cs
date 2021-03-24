using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Gig.Contracts.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;

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
                    o.Status
                })
                .FirstOrDefaultAsync(o => o.OrderNumber == @event.OrderNumber);

            orderStatus.Status.Reason = @event.Reason;

            _context.OrderStatus.Update(orderStatus.Status);
            await _context.SaveChangesAsync(new CancellationToken());
        }
    }
}