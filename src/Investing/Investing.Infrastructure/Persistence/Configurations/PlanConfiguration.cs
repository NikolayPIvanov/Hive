using Hive.Investing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Investing.Infrastructure.Persistence.Configurations
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.ToTable("plans", InvestingDbContext.Schema);
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Title).HasMaxLength(50).IsRequired();
            builder.Property(i => i.Description).HasMaxLength(5000).IsRequired();
            builder.Property(i => i.EstimatedReleaseDate).IsRequired(false);
            builder.Property(i => i.EstimatedReleaseDays).IsRequired();
            builder.Property(i => i.FundingNeeded)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.OwnsMany(p => p.Tags, t =>
            {
                t.WithOwner().HasForeignKey("PlanId");
                t.Property<int>("Id");
                t.HasKey("Id");
            });

            builder.HasOne(p => p.Investment)
                .WithOne()
                .HasForeignKey<Plan>(p => p.InvestmentId);
        }
    }
}