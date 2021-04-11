using Hive.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Infrastructure.Persistence.Configurations.Ordering
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
            builder.Ignore(x => x.IsClosed);

            builder.OwnsOne(o => o.Requirement, r =>
            {
                r.WithOwner().HasForeignKey("OrderId");
                r.Property<int>("Id");
                r.HasKey("Id");
            });
            
            builder.OwnsMany(o => o.OrderStates, os =>
            {
                os.WithOwner().HasForeignKey("OrderId");
                os.Property<int>("Id");
                os.HasKey("Id");
            });
            
            builder.HasMany(o => o.Resolutions)
                .WithOne()
                .HasForeignKey(r => r.OrderId);

            builder.HasOne(o => o.Seller)
                .WithMany(s => s.Orders)
                .HasForeignKey(r => r.SellerId);
            
            builder.HasOne(o => o.Buyer)
                .WithMany(b => b.Orders)
                .HasForeignKey(r => r.BuyerId);
            
        }
    }
}