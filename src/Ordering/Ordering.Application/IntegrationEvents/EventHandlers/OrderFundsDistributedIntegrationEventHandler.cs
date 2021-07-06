using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Billing.Contracts.IntegrationEvents;
using Investing.Contracts.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Domain.Enums;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.IntegrationEvents.EventHandlers
{
    public class OrderFundsDistributedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IOrderingContext _context;

        public OrderFundsDistributedIntegrationEventHandler(IOrderingContext context)
        {
            _context = context;
        }

        [CapSubscribe(nameof(OrderFundsDistributedIntegrationEvent), Group = "hive.ordering.funds.placed")]
        public async Task Handle(OrderFundsDistributedIntegrationEvent @event)
        {
            var resolution = await _context.Resolutions
                .Include(r => r.Order)
                .ThenInclude(o => o.Buyer)
                .Include(o => o.Order)
                .ThenInclude(o => o.OrderStates)
                .FirstOrDefaultAsync(x => x.Id == @event.ResolutionId);
            
            resolution.Order.OrderStates.Add(new State(OrderState.Completed, $"Buyer accepted resolution - {@event.ResolutionId}"));
            await _context.SaveChangesAsync(default);
        }
    }
}