using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Identity.Contracts.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Application.IntegrationEvents.EventHandlers
{
    public class BuyerCreatedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IOrderingContext _context;

        public BuyerCreatedIntegrationEventHandler(IOrderingContext context)
        {
            _context = context;
        }
        
        [CapSubscribe(nameof(BuyerCreatedIntegrationEvent), Group = "hive.ordering.buyers")]
        public async Task Handle(InvestorCreatedIntegrationEvent @event)
        {
            var alreadyRegistered = await _context.Buyers.AnyAsync(v => v.UserId == @event.UserId);
            if (alreadyRegistered)
                return;
            
            var investor = new Buyer(@event.UserId);

            _context.Buyers.Add(investor);
            await _context.SaveChangesAsync(default);
        }
    }
}