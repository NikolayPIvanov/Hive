namespace UserProfile.Application.Interfaces
{
    using Microsoft.EntityFrameworkCore;
    
    public interface IUserProfileContext
    {
        public DbSet<Hive.UserProfile.Domain.UserProfile> UserProfiles { get; set; }
    }
}