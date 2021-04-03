using Hive.Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Billing.Infrastructure.Persistence.Configurations
{
    public class AccountHolderConfiguration : IEntityTypeConfiguration<AccountHolder>
    {
        public void Configure(EntityTypeBuilder<AccountHolder> builder)
        {
            builder.ToTable("account_holders", BillingDbContext.Schema);
            builder.HasKey(a => a.Id);

            builder.HasIndex(h => h.UserId)
                .IsUnique();

            builder.HasOne(h => h.Account)
                .WithOne()
                .HasForeignKey<AccountHolder>(h => h.AccountId);
        }
    }
}