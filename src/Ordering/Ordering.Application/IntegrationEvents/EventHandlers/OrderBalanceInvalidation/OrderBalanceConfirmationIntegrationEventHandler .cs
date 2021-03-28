using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Billing.Contracts.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;

namespace Ordering.Application.IntegrationEvents.EventHandlers.OrderBalanceInvalidation
{
    public class OrderBalanceConfirmationIntegrationEventHandler : ICapSubscribe
    {
        private readonly IOrderingContext _context;

        public OrderBalanceConfirmationIntegrationEventHandler(IOrderingContext context)
        {
            _context = context;
        }
        
        [CapSubscribe(nameof(OrderBalanceConfirmationIntegrationEvent), Group = "cap.hive.ordering")] 
        public async Task Handle(OrderBalanceConfirmationIntegrationEvent @event)
        {
            var orderStatus = await _context.Orders
                .Select(o => new { o.Id, o.OrderNumber })
                .FirstOrDefaultAsync(o => o.OrderNumber == @event.OrderNumber);

            var state = new State(OrderState.UserBalanceValid, @event.Reason, orderStatus.Id);
            _context.OrderStates.Add(state);
            
            await _context.SaveChangesAsync(default);
        }
    }
}