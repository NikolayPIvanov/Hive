using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Configurations
{
    public class RequirementConfiguration : IEntityTypeConfiguration<Requirement>
    {
        public void Configure(EntityTypeBuilder<Requirement> builder)
        {
            builder.Property(r => r.Details).HasMaxLength(2500).IsRequired();
            builder.HasOne(r => r.Order)
                .WithOne(o => o.Requirement)
                .HasForeignKey<Requirement>(r => r.OrderId);
        }
    }
}