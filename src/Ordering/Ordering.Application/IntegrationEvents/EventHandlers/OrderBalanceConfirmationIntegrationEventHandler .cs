using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Gig.Contracts.IntegrationEvents;
using Hive.Billing.Contracts.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;

namespace Ordering.Application.IntegrationEvents.EventHandlers
{
    public class OrderBalanceConfirmationIntegrationEventHandler : ICapSubscribe
    {
        private readonly IOrderingContext _context;

        public OrderBalanceConfirmationIntegrationEventHandler(IOrderingContext context)
        {
            _context = context;
        }
        
        [CapSubscribe(nameof(OrderBalanceConfirmationIntegrationEvent))] 
        public async Task Handle(OrderBalanceConfirmationIntegrationEvent @event)
        {
            var orderStatus = await _context.Orders
                .Select(o => new
                {
                    o.OrderNumber,
                    o.OrderStates
                })
                .FirstOrDefaultAsync(o => o.OrderNumber == @event.OrderNumber);

            var state = new State(OrderState.UserBalanceValid, @event.Reason);
            orderStatus.OrderStates.Add(state);
            
            await _context.SaveChangesAsync(new CancellationToken());
        }
    }
}