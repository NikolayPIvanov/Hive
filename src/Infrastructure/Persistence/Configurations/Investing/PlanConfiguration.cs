using Hive.Domain.Entities.Investing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Infrastructure.Persistence.Configurations.Investing
{
    public class PlanConfiguration :  IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.Title).HasMaxLength(100).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(5000).IsRequired();
            
            // https://docs.microsoft.com/en-us/ef/core/modeling/owned-entities#collections-of-owned-types
            builder.OwnsMany(g => g.SearchTags, a =>
            {
                a.WithOwner().HasForeignKey("PlanId");
                a.Property<int>("Id");
                a.HasKey("Id");
            });

        }
    }
}