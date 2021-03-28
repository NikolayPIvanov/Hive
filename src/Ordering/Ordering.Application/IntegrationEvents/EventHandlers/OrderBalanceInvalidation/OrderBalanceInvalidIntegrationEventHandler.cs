using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Gig.Domain.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;

namespace Ordering.Application.IntegrationEvents.EventHandlers.OrderBalanceInvalidation
{
    public class OrderBalanceInvalidIntegrationEventHandler : ICapSubscribe
    {
        private readonly IOrderingContext _context;

        public OrderBalanceInvalidIntegrationEventHandler(IOrderingContext context)
        {
            _context = context;
        }

        [CapSubscribe(nameof(OrderBalanceInvalidIntegrationEvent), Group = "cap.hive.ordering")]
        public async Task Handle(OrderBalanceInvalidIntegrationEvent @event)
        {
            var orderStatus = await _context.Orders
                .Select(o => new {o.Id, o.OrderNumber})
                .FirstOrDefaultAsync(o => o.OrderNumber == @event.OrderNumber);

            var state = new State(OrderState.Invalid, @event.Reason, orderStatus.Id);
            _context.OrderStates.Add(state);

            await _context.SaveChangesAsync(default);
        }
    }
}