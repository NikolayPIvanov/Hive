namespace Hive.UserProfile.Application.Interfaces
{
    using Domain;
    
    using Microsoft.EntityFrameworkCore;

    public interface IUserProfileContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}