using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Common.Core.Exceptions;
using Hive.Identity.Contracts.IntegrationEvents;
using Hive.Identity.Data;
using Hive.Identity.Models;

namespace Hive.Identity.IntegrationEvents
{
    public class SellerStoredIntegrationEventHandler : ICapSubscribe
    {
        private readonly ApplicationDbContext _context;

        public SellerStoredIntegrationEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        [CapSubscribe(nameof(ConformationEvents.SellerStoredIntegrationEvent))]
        public async Task Handle(ConformationEvents.SellerStoredIntegrationEvent @event)
        {
            var user = await _context.Users.FindAsync(@event.UserId);
            if (user == null)
            {
                throw new NotFoundException(nameof(ApplicationUser), @event.UserId);
            }

            user.SellerId = @event.SellerId;
            await _context.SaveChangesAsync();
        }
    }
}