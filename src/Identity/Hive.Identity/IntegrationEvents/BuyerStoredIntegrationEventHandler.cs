using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Common.Core.Exceptions;
using Hive.Identity.Contracts.IntegrationEvents;
using Hive.Identity.Data;

namespace Hive.Identity.IntegrationEvents
{
    public class BuyerStoredIntegrationEventHandler : ICapSubscribe
    {
        private readonly ApplicationDbContext _context;

        public BuyerStoredIntegrationEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        [CapSubscribe(nameof(ConformationEvents.BuyerStoredIntegrationEvent))]
        public async Task Handle(ConformationEvents.BuyerStoredIntegrationEvent @event)
        {
            var user = await _context.Users.FindAsync(@event.UserId);
            if (user == null)
            {
                throw new NotFoundException(nameof(ApplicationDbContext), @event.UserId);
            }

            user.ExternalAccountId = @event.BuyerId;
            await _context.SaveChangesAsync();
        }
    }
}