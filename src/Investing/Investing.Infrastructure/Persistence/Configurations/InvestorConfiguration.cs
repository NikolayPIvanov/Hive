using Hive.Investing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Investing.Infrastructure.Persistence.Configurations
{
    public class InvestorConfiguration : IEntityTypeConfiguration<Investor>
    {
        public void Configure(EntityTypeBuilder<Investor> builder)
        {
            builder.ToTable("investors", InvestingDbContext.Schema);
            builder.HasKey(i => i.Id);

            builder.HasIndex(i => i.UserId).IsUnique();

            builder.HasMany(i => i.Investments)
                .WithOne()
                .HasForeignKey(i => i.InvestorId);
        }
    }
}