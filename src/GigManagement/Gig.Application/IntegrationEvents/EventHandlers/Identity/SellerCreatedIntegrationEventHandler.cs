using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Gig.Application.Interfaces;
using Hive.Identity.Contracts.IntegrationEvents;

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
        }
    }
}