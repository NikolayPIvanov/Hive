using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasIndex(o => o.OrderNumber).IsUnique();
            builder.Property(o => o.OrderedAt).IsRequired();
            builder.Property(o => o.UnitPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.PackageId).IsRequired();
            builder.Property(x => x.BuyerId).IsRequired(false);
            builder.Property(x => x.SellerUserId).IsRequired();

            builder.HasMany(o => o.Resolutions)
                .WithOne()
                .HasForeignKey(r => r.OrderId);

            builder.OwnsMany(o => o.OrderStates, os =>
            {
                os.WithOwner().HasForeignKey("OrderId");
                os.Property<int>("Id");
                os.HasKey("Id");
            });
            
            builder.OwnsOne(o => o.Requirement, os =>
            {
                os.ToTable("requirements");
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