using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Application.Interfaces;
using Hive.Common.Domain;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderingContext : DbContext, IOrderingContext
    {
        private readonly IDateTimeService _dateTimeService;
        private const string DefaultSchema = "ordering";
        
        public OrderingContext(
            DbContextOptions<OrderingContext> options,
            IDateTimeService dateTimeService) : base(options)
        {
            _dateTimeService = dateTimeService;
        }

        public string Schema => DefaultSchema;
        
        public DbSet<Order> Orders { get; set; }
        public DbSet<State> OrderStates { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<Resolution> Resolutions { get; set; }
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
         {
             foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
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