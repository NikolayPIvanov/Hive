using Hive.Seller.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Seller.Infrastructure.Configurations
{
    public class GigConfiguration : IEntityTypeConfiguration<Gig>
    {
        public void Configure(EntityTypeBuilder<Gig> builder)
        {
            builder.Property(g => g.Title).HasMaxLength(50).IsRequired();
            builder.Property(g => g.Metadata).HasMaxLength(50);
            builder.Property(g => g.Tags).HasMaxLength(100).IsRequired();

            builder.HasOne(g => g.Category)
                .WithOne()
                .HasForeignKey<Gig>(g => g.CategoryId);
            
            builder.HasMany(g => g.Packages)
                .WithOne(p => p.Gig)
                .HasForeignKey(g => g.GigId);
        }
    }
}