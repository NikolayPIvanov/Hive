using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.SeedWork;
using Hive.Common.Domain;
using Hive.Common.Domain.SeedWork;
using Hive.Gig.Application.Questions.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using IDateTimeService = Hive.Common.Core.Interfaces.IDateTimeService;

namespace Hive.Gig.Infrastructure.Persistence
{
    using Domain.Entities;
    
    public class GigManagementDbContext : DbContext, IGigManagementDbContext
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;

        public GigManagementDbContext(
            DbContextOptions<GigManagementDbContext> options,
            IMediator mediator,
            ICurrentUserService currentUserService,
            IDateTimeService dateTimeService) : base(options)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Gig> Gigs { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
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
            builder.HasDefaultSchema("gmt");
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}