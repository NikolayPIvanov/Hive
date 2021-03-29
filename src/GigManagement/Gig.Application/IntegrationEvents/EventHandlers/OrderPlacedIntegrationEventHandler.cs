using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Common.Application.Interfaces;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Contracts.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Ordering.Contracts.IntegrationEvents;

namespace Hive.Gig.Application.IntegrationEvents.EventHandlers
{
    public class OrderPlacedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IGigManagementContext _context;
        private readonly IIntegrationEventPublisher _publisher;

        public OrderPlacedIntegrationEventHandler(IGigManagementContext context, IIntegrationEventPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }
        
        [CapSubscribe(nameof(OrderPlacedIntegrationEvent))] 
        public async Task Handle(OrderPlacedIntegrationEvent @event)
        {
            var gig = await _context.Gigs
                .Include(g => g.Packages)
                .FirstOrDefaultAsync(g => g.Id == @event.GigId);
            
            var reason = $"Gig with id {@event.GigId} was not found";
            var integrationEvent = new OrderValidatedIntegrationEvent(@event.OrderNumber, reason, IsValid: false);
            
            if (gig is null)
            {
                var invalidationEvent = new OrderValidatedIntegrationEvent(@event.OrderNumber, reason);
                await _publisher.Publish(invalidationEvent);
                return;
            }
            
            var package = gig.Packages.FirstOrDefault(p => p.Id == @event.PackageId);
            if (package is null)
            {
                reason = $"Package with id {@event.PackageId} was not found for gig with id {@event.GigId}";
                await _publisher.Publish(integrationEvent  with { Reason = reason});
                return;
            }

            var priceIsSame = package.Price == @event.UnitPrice;
            if (!priceIsSame)
            {
                reason = $"Order for package with id {@event.PackageId} was passed with price {@event.UnitPrice} but it was {package.Price}";
                await _publisher.Publish(integrationEvent  with { Reason = reason});
                return;
            }

            var sellerIdIsValid = gig.SellerId == @event.SellerId;
            if (!sellerIdIsValid) 
            {
                reason = $"Order {@event.OrderNumber} had invalid seller id {@event.SellerId}";
                await _publisher.Publish(integrationEvent  with { Reason = reason});
                return;
            }

            reason = "Order details are valid.";
            await _publisher.Publish(integrationEvent  with { Reason = reason, IsValid = true});
        }
    }
}