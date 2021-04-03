using Hive.Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Billing.Infrastructure.Persistence.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("accounts", BillingDbContext.Schema);
            builder.HasKey(a => a.Id);

            builder.HasMany(a => a.PaymentMethods)
                .WithOne()
                .HasForeignKey(pm => pm.AccountId);
        }
    }
}