using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasAlternateKey(o => o.OrderNumber).IsClustered(false);
            builder.Property(o => o.OrderedAt).IsRequired();
            builder.Property(o => o.UnitPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.PackageId).IsRequired();
            builder.Property(x => x.BuyerId).IsRequired();
            builder.Property(x => x.SellerUserId).IsRequired();

            builder.HasMany(o => o.Resolutions)
                .WithOne(r => r.Order)
                .HasForeignKey(r => r.OrderId);

            builder.OwnsMany(o => o.OrderStates, os =>
            {
                os.ToTable("OrderStates");
                os.WithOwner().HasForeignKey("OrderId");
                os.Property<int>("Id");
                os.HasKey("Id");
            });
            
            builder.OwnsOne(o => o.Requirement, os =>
            {
                os.ToTable("Requirements");
                os.Property(r => r.Details)
                    .HasMaxLength(2500)
                    .IsRequired();

                os.WithOwner().HasForeignKey("OrderId");
                os.Property<int>("Id");
                os.HasKey("Id");
            });
        }
    }
}