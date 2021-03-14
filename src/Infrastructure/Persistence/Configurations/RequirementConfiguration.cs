using Hive.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Infrastructure.Persistence.Configurations
{
    public class RequirementConfiguration : IEntityTypeConfiguration<Requirement>
    {
        public void Configure(EntityTypeBuilder<Requirement> builder)
        {
            builder.Property(r => r.Details).HasMaxLength(1000).IsRequired();
            builder.HasOne(r => r.Order)
                .WithOne(o => o.Requirement)
                .HasForeignKey<Requirement>(r => r.OrderId);
        }
    }
}