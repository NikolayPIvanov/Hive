using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Identity.Contracts.IntegrationEvents;
using Hive.UserProfile.Application.Interfaces;

namespace Hive.UserProfile.Application.IntegrationEvents.EventHandlers.Identity
{
    public class UserCreatedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IUserProfileContext _context;

        public UserCreatedIntegrationEventHandler(IUserProfileContext context)
        {
            _context = context;
        }

        [CapSubscribe(nameof(UserCreatedIntegrationEvent))]
        public async Task Handle(UserCreatedIntegrationEvent @event)
        {
            var profile = new Domain.UserProfile(@event.UserId);

            _context.UserProfiles.Add(profile);
            await _context.SaveChangesAsync();
        }
    }
}