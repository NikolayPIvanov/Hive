using Hive.Domain.Entities.Investments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Infrastructure.Persistence.Configurations
{
    public class PlanConfiguration :  IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.Title).HasMaxLength(100).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(5000).IsRequired();
            builder.Property(p => p.IsReleased).HasDefaultValue(false);

            // builder.HasOne(p => p.Seller)
            //     .WithMany(s => s.Plans)
            //     .HasForeignKey(p => p.SellerId);

            builder.HasMany(x => x.Investments)
                .WithOne(x => x.Plan)
                .HasForeignKey(x => x.PlanId);
        }
    }
}