namespace Hive.UserProfile.Application.Interfaces
{
    using Domain;
    
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public interface IUserProfileDbContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());
    }
}