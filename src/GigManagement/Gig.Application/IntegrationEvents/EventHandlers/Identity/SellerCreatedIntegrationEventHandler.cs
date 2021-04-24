using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Common.Core.Interfaces;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using Hive.Identity.Contracts.IntegrationEvents;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.IntegrationEvents.EventHandlers.Identity
{
    public class SellerCreatedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly IIntegrationEventPublisher _publisher;

        public SellerCreatedIntegrationEventHandler(IGigManagementDbContext dbContext, IIntegrationEventPublisher publisher)
        {
            _dbContext = dbContext;
            _publisher = publisher;
        }
        
        [CapSubscribe(nameof(SellerCreatedIntegrationEvent))]
        public async Task Handle(SellerCreatedIntegrationEvent @event)
        {
            var sellerIsRegistered = await _dbContext.Sellers.AnyAsync(s => s.UserId == @event.UserId);
            if (sellerIsRegistered)
                return;

            var seller = new Seller(@event.UserId);
            _dbContext.Sellers.Add(seller);

            var message = new ConformationEvents.SellerStoredIntegrationEvent(@event.UserId, seller.Id, true);
            await _publisher.Publish(message);
            
            await _dbContext.SaveChangesAsync(default);
            
        }
    }
}