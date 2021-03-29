using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using Hive.Identity.Contracts.IntegrationEvents;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.IntegrationEvents.EventHandlers.Identity
{
    public class SellerCreatedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IGigManagementContext _context;

        public SellerCreatedIntegrationEventHandler(IGigManagementContext context)
        {
            _context = context;
        }
        
        // TODO: Refactor
        [CapSubscribe(nameof(SellerCreatedIntegrationEvent))]
        public async Task Handle(SellerCreatedIntegrationEvent @event)
        {
            var sellerIsRegistered = await _context.Sellers.AnyAsync(s => s.UserId == @event.UserId);
            if (!sellerIsRegistered)
            {
                return;
            }

            var seller = new Seller(@event.UserId);
            _context.Sellers.Add(seller);
            await _context.SaveChangesAsync(default);
        }
    }
}