using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Gig.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Ordering.Contracts.IntegrationEvents;

namespace Hive.Gig.Application.IntegrationEvents
{
    public class OrderCreatedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IGigManagementContext _context;

        public OrderCreatedIntegrationEventHandler(IGigManagementContext context)
        {
            _context = context;
        }
        
        [CapSubscribe(nameof(OrderCreatedIntegrationEvent))] 
        public async Task Handle(OrderCreatedIntegrationEvent @event)
        {
            var gig = await _context.Gigs
                .Include(g => g.Packages)
                // TODO: Include seller
                .FirstOrDefaultAsync(g => g.Id == @event.GigId);

            if (gig is null)
            {
                await Task.CompletedTask;
                return;
            }
            
            var package = gig.Packages.FirstOrDefault(p => p.Id == @event.PackageId);
            if (package is null)
            {
                return;
            }

            var priceIsSame = package.Price == @event.UnitPrice;
            if (!priceIsSame)
            {
                return;
            }
            
            

        }
    }
}