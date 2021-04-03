using System.ComponentModel;
using Hive.Investing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Investing.Infrastructure.Persistence.Configurations
{
    public class InvestmentConfiguration : IEntityTypeConfiguration<Investment>
    {
        public void Configure(EntityTypeBuilder<Investment> builder)
        {
            builder.ToTable("investments", InvestingDbContext.Schema);
            builder.HasKey(i => i.Id);

            builder.Ignore(i => i.DomainEvents);

            builder.Property(i => i.EffectiveDate).IsRequired();
            builder.Property(i => i.ExpirationDate).IsRequired(false);
            builder.Property(i => i.Amount).IsRequired();
            builder.Property(i => i.RoiPercentage).IsRequired();
        }
    }
}