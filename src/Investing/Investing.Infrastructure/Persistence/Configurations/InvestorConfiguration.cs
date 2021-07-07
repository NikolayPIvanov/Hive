using Hive.Investing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Investing.Infrastructure.Persistence.Configurations
{
    public class InvestorConfiguration : IEntityTypeConfiguration<Investor>
    {
        public void Configure(EntityTypeBuilder<Investor> builder)
        {
            builder.HasAlternateKey(x => x.UserId).IsClustered(false);
            builder.Ignore(x => x.DomainEvents);

            builder.HasMany(x => x.Investments)
                .WithOne(x => x.Investor)
                .HasForeignKey(x => x.InvestorId);
        }
    }
}