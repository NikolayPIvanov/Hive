using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Application.Interfaces;
using Hive.Common.Domain;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Hive.UserProfile.Infrastructure.Persistence
{
    using Domain;
    using Application.Interfaces;
    
    using Microsoft.EntityFrameworkCore;

    public class UserProfileContext : DbContext, IUserProfileContext
    {
        private readonly IDateTimeService _dateTimeService;
        private const string DefaultSchema = "userprofile";
        
        public UserProfileContext(
            DbContextOptions<UserProfileContext> options,
            IDateTimeService dateTimeService) : base(options)
        {
            _dateTimeService = dateTimeService;
        }

        public static string Schema => DefaultSchema;
        
        public DbSet<UserProfile> UserProfiles { get; set; }
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = null;
                        entry.Entity.Created = _dateTimeService.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = null;
                        entry.Entity.LastModified = _dateTimeService.Now;
                        break;
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);
            
            return result;
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(DefaultSchema);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}