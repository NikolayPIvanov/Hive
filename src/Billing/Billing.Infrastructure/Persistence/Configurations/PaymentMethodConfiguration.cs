using Hive.Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Billing.Infrastructure.Persistence.Configurations
{
    public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.ToTable("payment_methods", BillingDbContext.Schema);
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Alias).HasMaxLength(50).IsRequired();

            // builder.HasOne(m => m.Account)
            //     .WithOne(a => a.DefaultPaymentMethod)
            //     .HasForeignKey<Account>(a => a.DefaultPaymentMethodId);
            //
            builder.HasOne(m => m.Account)
                .WithMany(a => a.PaymentMethods)
                .HasForeignKey(a => a.AccountId);

            builder.HasMany(m => m.Transactions)
                .WithOne()
                .HasForeignKey(t => t.PaymentMethodId);
        }
    }
}