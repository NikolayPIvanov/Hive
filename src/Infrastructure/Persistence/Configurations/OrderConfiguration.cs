using Hive.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Infrastructure.Persistence.Configurations
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

            builder.HasOne(o => o.Requirement)
                .WithOne()
                .HasForeignKey<Order>(o => o.RequirementId);
            
            builder.HasMany(o => o.Resolutions)
                .WithOne()
                .HasForeignKey(r => r.OrderId);

            builder.OwnsMany(o => o.OrderStates, os =>
            {
                os.WithOwner().HasForeignKey("OrderId");
                os.Property<int>("Id");
                os.HasKey("Id");
            });
            
        }
    }
}