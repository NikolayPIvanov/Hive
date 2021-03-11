using Hive.Domain.Entities.Gigs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Infrastructure.Persistence.Configurations
{
    public class PackageConfiguration : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(p => p.Title).HasMaxLength(50).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(75).IsRequired();
            builder.Property(p => p.DeliveryTime).IsRequired();
            builder.Property(p => p.DeliveryFrequency).IsRequired();
            builder.Property(p => p.PackageTier).IsRequired();

            builder.HasOne(p => p.Gig)
                .WithMany(g => g.Packages)
                .HasForeignKey(p => p.GigId);
        }
    }
}