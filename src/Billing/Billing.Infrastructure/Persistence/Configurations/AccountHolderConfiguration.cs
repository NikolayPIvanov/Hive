using Hive.Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Billing.Infrastructure.Persistence.Configurations
{
    public class AccountHolderConfiguration : IEntityTypeConfiguration<AccountHolder>
    {
        public void Configure(EntityTypeBuilder<AccountHolder> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasAlternateKey(x => x.UserId).IsClustered(false);
            builder.Ignore(x => x.DomainEvents);

            builder.HasOne(x => x.Wallet)
                .WithOne(x => x.AccountHolder)
                .HasForeignKey<AccountHolder>(x => x.WalletId);
        }
    }
}