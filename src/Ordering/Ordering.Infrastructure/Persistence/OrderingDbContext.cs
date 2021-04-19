using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.SeedWork;
using Hive.Common.Domain;
using Hive.Common.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderingDbContext : DbContext, IOrderingContext
    {
        private readonly IDateTimeService _dateTimeService;
        private const string DefaultSchema = "ordering";
        
        public OrderingDbContext(
            DbContextOptions<OrderingDbContext> options,
            IDateTimeService dateTimeService) : base(options)
        {
            _dateTimeService = dateTimeService;
        }
        
        public DbSet<Order> Orders { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<Resolution> Resolutions { get; set; }
        public DbSet<Buyer> Buyers { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
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
            builder.HasDefaultSchema(DefaultSchema);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}