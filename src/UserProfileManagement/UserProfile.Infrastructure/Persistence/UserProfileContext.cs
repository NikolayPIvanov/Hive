namespace Hive.UserProfile.Infrastructure.Persistence
{
    using Domain;
    using Application.Interfaces;
    
    using Microsoft.EntityFrameworkCore;

    public class UserProfileContext : IUserProfileContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}