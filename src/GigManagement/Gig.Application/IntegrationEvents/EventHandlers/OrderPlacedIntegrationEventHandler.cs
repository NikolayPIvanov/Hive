using System;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using DotNetCore.CAP;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Contracts.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Ordering.Contracts.IntegrationEvents;

namespace Hive.Gig.Application.IntegrationEvents.EventHandlers
{
    public class OrderPlacedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly IIntegrationEventPublisher _publisher;

        public OrderPlacedIntegrationEventHandler(IGigManagementDbContext dbContext, IIntegrationEventPublisher publisher)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        }
        
        [CapSubscribe(nameof(OrderPlacedIntegrationEvent))] 
        public async Task Handle(OrderPlacedIntegrationEvent @event)
        {
            var package = await _dbContext.Packages
                .FirstOrDefaultAsync(g => g.Id == @event.PackageId);
            
            var reason = $"Package with id {@event.PackageId} was not found";
            var integrationEvent = new OrderValidatedIntegrationEvent(@event.OrderNumber, reason, IsValid: false);
            
            if (package is null)
            {
                var invalidationEvent = new OrderValidatedIntegrationEvent(@event.OrderNumber, reason);
                await _publisher.PublishAsync(invalidationEvent);
                return;
            }

            var priceIsSame = package.Price == @event.UnitPrice;
            if (!priceIsSame)
            {
                reason = $"Order for package with id {@event.PackageId} was passed with price {@event.UnitPrice} but it was {package.Price}";
                await _publisher.PublishAsync(integrationEvent  with { Reason = reason});
                return;
            }

            var gig = await _dbContext.Gigs
                .Include(x => x.Seller)
                .FirstOrDefaultAsync(x => x.Id ==package.GigId, default);
            
            var sellerIdIsValid = gig.Seller.UserId == @event.SellerUserId;
            if (!sellerIdIsValid) 
            {
                reason = $"Order {@event.OrderNumber} had invalid seller id {@event.SellerUserId}";
                await _publisher.PublishAsync(integrationEvent  with { Reason = reason});
                return;
            }

            reason = "Order details are valid.";
            await _publisher.PublishAsync(integrationEvent  with { Reason = reason, IsValid = true});
        }
    }
}