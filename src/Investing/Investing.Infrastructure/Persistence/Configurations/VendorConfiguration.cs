using Hive.Investing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Investing.Infrastructure.Persistence.Configurations
{
    public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder.ToTable("vendors", InvestingDbContext.Schema);
            builder.HasKey(i => i.Id);

            builder.HasIndex(i => i.UserId).IsUnique();

            builder.HasMany(i => i.Plans)
                .WithOne()
                .HasForeignKey(i => i.VendorId);
        }
    }
}