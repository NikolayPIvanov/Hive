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
        private readonly IGigManagementDbContext _dbContext;

        public SellerCreatedIntegrationEventHandler(IGigManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        // TODO: Refactor
        [CapSubscribe(nameof(SellerCreatedIntegrationEvent))]
        public async Task Handle(SellerCreatedIntegrationEvent @event)
        {
            var sellerIsRegistered = await _dbContext.Sellers.AnyAsync(s => s.UserId == @event.UserId);
            if (!sellerIsRegistered)
            {
                return;
            }

            var seller = new Seller(@event.UserId);
            _dbContext.Sellers.Add(seller);
            await _dbContext.SaveChangesAsync(default);
        }
    }
}