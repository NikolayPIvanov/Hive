using Hive.Investing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Investing.Infrastructure.Persistence.Configurations
{
    public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder.HasAlternateKey(x => x.UserId).IsClustered(false);
            builder.Ignore(x => x.DomainEvents);

            builder.HasMany(x => x.Plans)
                .WithOne(p => p.Vendor)
                .HasForeignKey(x => x.VendorId);
        }
    }
}