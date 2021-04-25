using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.SeedWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderingDbContext : DbContext, IOrderingContext
    {
        private const string DefaultSchema = "ordering";

        private readonly IMediator _mediator;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICurrentUserService _currentUserService;
        
        public OrderingDbContext(
            DbContextOptions<OrderingDbContext> options,
            IMediator mediator,
            ICurrentUserService currentUserService,
            IDateTimeService dateTimeService) : base(options)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
        }
        
        public DbSet<Order> Orders { get; set; }
        public DbSet<Resolution> Resolutions { get; set; }
        public DbSet<Buyer> Buyers { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
         {
             foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Entity> entry in ChangeTracker.Entries<Entity>())
             {
                 switch (entry.State)
                 {
                     case EntityState.Added:
                         entry.Entity.CreatedBy = _currentUserService.UserId;
                         entry.Entity.Created = _dateTimeService.Now;
                         break;

                     case EntityState.Modified:
                         entry.Entity.LastModifiedBy = _currentUserService.UserId;
                         entry.Entity.LastModified = _dateTimeService.Now;
                         break;
                 }
             }

             var result = await base.SaveChangesAsync(cancellationToken);

             await _mediator.DispatchDomainEventsAsync(this);

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