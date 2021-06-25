using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Common.Core.Exceptions;
using Hive.Identity.Contracts.IntegrationEvents;
using Hive.Identity.Data;

namespace Hive.Identity.IntegrationEvents
{
    public class InvestorStoredIntegrationEventHandler : ICapSubscribe
    {
        private readonly ApplicationDbContext _context;

        public InvestorStoredIntegrationEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        [CapSubscribe(nameof(ConformationEvents.InvestorStoredIntegrationEvent))]
        public async Task Handle(ConformationEvents.InvestorStoredIntegrationEvent @event)
        {
            var user = await _context.Users.FindAsync(@event.UserId);
            if (user == null)
            {
                throw new NotFoundException(nameof(ApplicationDbContext), @event.UserId);
            }

            user.ExternalAccountId = @event.InvestorId;
            await _context.SaveChangesAsync();
        }
    }
}