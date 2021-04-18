using Hive.Domain.Entities.Investing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Infrastructure.Persistence.Configurations.Investing
{
    public class InvestmentConfiguration : IEntityTypeConfiguration<Investment>
    {
        public void Configure(EntityTypeBuilder<Investment> builder)
        {
            builder.Property(x => x.RoiPercentage).IsRequired();
            builder.Property(x => x.Amount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.ExpirationDate).IsRequired(false);

            builder.HasOne(x => x.Investor)
                .WithMany(x => x.Investments)
                .HasForeignKey(x => x.InvestorId);
        }
    }
}