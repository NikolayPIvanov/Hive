using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Domain;
using Hive.Common.Domain.SeedWork;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using IDateTimeService = Hive.Common.Core.Interfaces.IDateTimeService;

namespace Hive.Gig.Infrastructure.Persistence
{
    public class GigManagementDbContext : DbContext, IGigManagementDbContext
    {
        private readonly IDateTimeService _dateTimeService;

        public GigManagementDbContext(
            DbContextOptions<GigManagementDbContext> options,
            IDateTimeService dateTimeService) : base(options)
        {
            _dateTimeService = dateTimeService;
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Domain.Entities.Gig> Gigs { get; set; }
        public DbSet<GigScope> GigScopes { get; set; }
        public DbSet<Package> Packages { get; set; }
        
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Question> Questions { get; set; }

        public DbSet<Seller> Sellers { get; set; }
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Entity> entry in ChangeTracker.Entries<Entity>())
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

            //await DispatchEvents();

            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("gmt");
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}