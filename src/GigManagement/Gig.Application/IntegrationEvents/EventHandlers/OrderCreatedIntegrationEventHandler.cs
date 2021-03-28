using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Common.Application.Interfaces;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Contracts.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Ordering.Contracts.IntegrationEvents;

namespace Hive.Gig.Application.IntegrationEvents
{
    public class OrderCreatedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IGigManagementContext _context;
        private readonly IIntegrationEventPublisher _publisher;

        public OrderCreatedIntegrationEventHandler(IGigManagementContext context, IIntegrationEventPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }
        
        // TODO: Refactor
        [CapSubscribe(nameof(OrderCreatedIntegrationEvent))] 
        public async Task Handle(OrderCreatedIntegrationEvent @event)
        {
            var reason = $"Gig with id {@event.GigId} was not found";
            var gig = await _context.Gigs
                .Include(g => g.Packages)
                // TODO: Include seller
                .FirstOrDefaultAsync(g => g.Id == @event.GigId);
        
            if (gig is null)
            {
                var invalidationEvent = new OrderInvalidIntegrationEvent(@event.OrderNumber, reason);
                await _publisher.Publish(invalidationEvent);
                return;
            }
            
            var package = gig.Packages.FirstOrDefault(p => p.Id == @event.PackageId);
            if (package is null)
            {
                reason = $"Package with id {@event.PackageId} was not found for gig with id {@event.GigId}";
                var invalidationEvent = new OrderInvalidIntegrationEvent(@event.OrderNumber, reason);
                await _publisher.Publish(invalidationEvent);
                return;
            }

            var priceIsSame = package.Price == @event.UnitPrice;
            if (!priceIsSame)
            {
                reason = $"Order for package with id {@event.PackageId} was passed with price {@event.UnitPrice} but it was {package.Price}";
                var invalidationEvent = new OrderInvalidIntegrationEvent(@event.OrderNumber, reason);
                await _publisher.Publish(invalidationEvent);
                return;
            }

            var confirmationEvent = new OrderDataConfirmationIntegrationEvent(@event.OrderNumber, "Order details are valid.");
            await _publisher.Publish(confirmationEvent);
        }
    }
}