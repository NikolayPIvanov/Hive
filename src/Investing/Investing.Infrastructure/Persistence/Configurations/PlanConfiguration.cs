using Hive.Investing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Investing.Infrastructure.Persistence.Configurations
{
    public class PlanConfiguration :  IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.Title).HasMaxLength(50).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(3000).IsRequired();
            builder.Property(p => p.StartDate).IsRequired();
            builder.Property(p => p.EndDate).IsRequired();

            // https://docs.microsoft.com/en-us/ef/core/modeling/owned-entities#collections-of-owned-types

            builder.HasMany(x => x.Investments)
                .WithOne(i => i.Plan)
                .HasForeignKey(i => i.PlanId);

        }
    }
}