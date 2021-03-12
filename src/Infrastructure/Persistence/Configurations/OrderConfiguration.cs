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
            builder.Property(o => o.OrderedById).IsRequired();
            builder.Property(o => o.IsCanceled).HasDefaultValue(false).IsRequired();
            builder.Property(o => o.OfferedById).IsRequired();
            builder.Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        }
    }
}