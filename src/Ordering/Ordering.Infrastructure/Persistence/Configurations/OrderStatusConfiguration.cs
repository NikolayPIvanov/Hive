using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Configurations
{
    public class OrderStatusConfiguration : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.HasOne(s => s.Order)
                .WithMany(o => o.OrderStates)
                .HasForeignKey(s => s.OrderId);
        }
    }
}