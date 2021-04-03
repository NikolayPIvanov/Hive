using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using Hive.Billing.Domain;
using Hive.Billing.Domain.Entities;
using Hive.Common.Core.Interfaces;
using Hive.Common.Domain;
using Hive.Common.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Billing.Infrastructure
{
    public class BillingDbContext : DbContext, IBillingDbContext
    {
        private readonly IDateTimeService _dateTimeService;
        private const string DefaultSchema = "billing";
        
        public BillingDbContext(
            DbContextOptions<BillingDbContext> options,
            IDateTimeService dateTimeService) : base(options)
        {
            _dateTimeService = dateTimeService;
        }

        public string Schema => DefaultSchema;
        
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountHolder> AccountHolders { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        
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