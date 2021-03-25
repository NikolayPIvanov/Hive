using System.Reflection;
using Billing.Application.Interfaces;
using Billing.Domain;
using Microsoft.EntityFrameworkCore;

namespace Billing.Infrastructure
{
    public class BillingContext : DbContext, IBillingContext
    {
        private const string DefaultSchema = "billing";
        
        public BillingContext(
            DbContextOptions<BillingContext> options) : base(options)
        {
        }

        public string Schema => DefaultSchema;
        
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(DefaultSchema);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}