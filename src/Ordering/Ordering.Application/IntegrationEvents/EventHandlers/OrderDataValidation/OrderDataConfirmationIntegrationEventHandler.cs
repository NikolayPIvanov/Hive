using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Gig.Contracts.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;

namespace Ordering.Application.IntegrationEvents.EventHandlers.OrderDataValidation
{
    public class OrderDataConfirmationIntegrationEventHandler : ICapSubscribe
    {
        private readonly IOrderingContext _context;

        public OrderDataConfirmationIntegrationEventHandler(IOrderingContext context)
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