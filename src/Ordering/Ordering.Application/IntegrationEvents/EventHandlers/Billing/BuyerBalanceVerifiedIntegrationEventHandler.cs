using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Billing.Contracts.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Domain.Enums;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.IntegrationEvents.EventHandlers.Billing
{
    public class BuyerBalanceVerifiedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IOrderingContext _context;

        public BuyerBalanceVerifiedIntegrationEventHandler(IOrderingContext context)
        {
            _context = context;
        }

        [CapSubscribe(nameof(BuyerBalanceVerifiedIntegrationEvent), Group = "cap.hive.ordering")]
        public async Task Handle(BuyerBalanceVerifiedIntegrationEvent @event)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderNumber == @event.OrderNumber);

            var orderState = @event.IsValid ? OrderState.UserBalanceValid : OrderState.Invalid;
            var state = new State(orderState, @event.Reason);
            order.OrderStates.Add(state);

            await _context.SaveChangesAsync(default);
        }
    }
}