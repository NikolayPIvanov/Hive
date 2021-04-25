using Hive.Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Billing.Infrastructure.Persistence.Configurations
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Ignore(x => x.DomainEvents);
            builder.Property(x => x.Balance).HasColumnType("decimal(18,2)").IsRequired();
            
            builder.HasMany(x => x.Transactions)
                .WithOne(x => x.Wallet)
                .HasForeignKey(x => x.WalletId);
        }
    }
}