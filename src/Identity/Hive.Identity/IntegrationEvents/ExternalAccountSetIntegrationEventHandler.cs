using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Common.Core.Exceptions;
using Hive.Identity.Contracts.IntegrationEvents;
using Hive.Identity.Contracts.IntegrationEvents.Inbound;
using Hive.Identity.Data;

namespace Hive.Identity.IntegrationEvents
{
    public class ExternalAccountSetIntegrationEventHandler : ICapSubscribe
    {
        private readonly ApplicationDbContext _context;

        public ExternalAccountSetIntegrationEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        [CapSubscribe(nameof(ExternalAccountSetIntegrationEvent))]
        public async Task Handle(ExternalAccountSetIntegrationEvent @event)
        {
            var user = await _context.Users.FindAsync(@event.UserId);
            if (user == null)
            {
                throw new NotFoundException(nameof(ApplicationDbContext), @event.UserId);
            }

            user.ExternalAccountId = @event.ExternalAccountId;
            await _context.SaveChangesAsync();
        }
    }
}