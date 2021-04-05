using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using Hive.Billing.Domain.Entities;
using Hive.Common.Core.Interfaces;
using Hive.Common.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Billing.Infrastructure.Persistence
{
    public class BillingDbContext : DbContext, IBillingDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;
        private const string DefaultSchema = "billing";
        
        public BillingDbContext(
            DbContextOptions<BillingDbContext> options,
            ICurrentUserService currentUserService,
            IDateTimeService dateTimeService) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
        }

        public static string Schema => DefaultSchema;
        
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountHolder> AccountHolders { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        
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