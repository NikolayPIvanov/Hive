using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Common.Core.Exceptions;
using Hive.Identity.Contracts.IntegrationEvents;
using Hive.Identity.Contracts.IntegrationEvents.Inbound;
using Hive.Identity.Data;
using Hive.Identity.Models;
using Microsoft.Extensions.Logging;

namespace Hive.Identity.IntegrationEvents
{
    public class ExternalAccountSetIntegrationEventHandler : ICapSubscribe
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ExternalAccountSetIntegrationEventHandler> _logger;

        public ExternalAccountSetIntegrationEventHandler(ApplicationDbContext context, ILogger<ExternalAccountSetIntegrationEventHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        [CapSubscribe(nameof(ExternalAccountSetIntegrationEvent))]
        public async Task Handle(ExternalAccountSetIntegrationEvent @event)
        {
            var user = await _context.Users.FindAsync(@event.UserId);
            if (user == null)
            {
                _logger.LogError($"User with id={@event.UserId} is missing");
                return;
            }

            user.ExternalAccountId = @event.ExternalAccountId;
            await _context.SaveChangesAsync();
        }
    }
}