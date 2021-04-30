using System;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using DotNetCore.CAP;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using Hive.Identity.Contracts.IntegrationEvents;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.IntegrationEvents.EventHandlers
{
    public class SellerCreatedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly IIntegrationEventPublisher _publisher;

        public SellerCreatedIntegrationEventHandler(IGigManagementDbContext dbContext, IIntegrationEventPublisher publisher)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        }
        
        [CapSubscribe(nameof(SellerCreatedIntegrationEvent))]
        public async Task Handle(SellerCreatedIntegrationEvent @event)
        {
            var sellerIsRegistered = await _dbContext.Sellers.AnyAsync(s => s.UserId == @event.UserId);
            if (sellerIsRegistered)
                return;

            var seller = new Seller(@event.UserId);
            _dbContext.Sellers.Add(seller);

            await _dbContext.SaveChangesAsync(default);
            
            var message = new ConformationEvents.SellerStoredIntegrationEvent(@event.UserId, seller.Id, true);
            await _publisher.PublishAsync(message);
        }
    }
}