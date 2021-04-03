using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Identity.Contracts.IntegrationEvents;
using Hive.UserProfile.Application.Interfaces;

namespace Hive.UserProfile.Application.IntegrationEvents.EventHandlers.Identity
{
    public class UserCreatedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IUserProfileDbContext _dbContext;

        public UserCreatedIntegrationEventHandler(IUserProfileDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [CapSubscribe(nameof(UserCreatedIntegrationEvent))]
        public async Task Handle(UserCreatedIntegrationEvent @event)
        {
            var profile = new Domain.UserProfile(@event.UserId);

            _dbContext.UserProfiles.Add(profile);
            await _dbContext.SaveChangesAsync();
        }
    }
}