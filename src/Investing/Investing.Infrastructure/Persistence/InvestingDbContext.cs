using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.SeedWork;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Investing.Infrastructure.Persistence
{
    public class InvestingDbContext : DbContext, IInvestingDbContext
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;
        private const string DefaultSchema = "investing";
        
        public InvestingDbContext(
            DbContextOptions<InvestingDbContext> options,
            IMediator mediator,
            ICurrentUserService currentUserService,
            IDateTimeService dateTimeService) : base(options)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
        }

        public static string Schema => DefaultSchema;
        
        public DbSet<Investor> Investors { get; set; }
        public DbSet<Investment> Investments { get; set; }
        public DbSet<Plan> Plans { get; set; }
        
        public DbSet<Vendor> Vendors { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Entity> entry in ChangeTracker
                .Entries<Entity>())
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