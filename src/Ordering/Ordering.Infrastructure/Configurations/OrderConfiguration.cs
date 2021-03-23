using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasIndex(o => o.OrderNumber).IsUnique();
            builder.Property(o => o.OrderedAt).IsRequired();
            // builder.Property(o => o.OrderedById).IsRequired();
            // builder.Property(o => o.IsCanceled).HasDefaultValue(false).IsRequired();
            // builder.Property(o => o.OfferedById).IsRequired();
            builder.Property(o => o.UnitAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(o => o.Requirement)
                .WithOne(r => r.Order)
                .HasForeignKey<Order>(o => o.RequirementId);

        }
    }
}