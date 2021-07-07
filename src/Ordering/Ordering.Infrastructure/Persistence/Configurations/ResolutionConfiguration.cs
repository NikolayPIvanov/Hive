using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence.Configurations
{
    public class ResolutionConfiguration : IEntityTypeConfiguration<Resolution>
    {
        public void Configure(EntityTypeBuilder<Resolution> builder)
        {
            builder.Property(x => x.Version).IsRequired();
            builder.Property(x => x.Location).IsRequired();
            builder.Property(x => x.IsApproved).IsRequired();

            builder.HasOne(r => r.Order)
                .WithMany(o => o.Resolutions)
                .HasForeignKey(x => x.OrderId);
        }
    }
}