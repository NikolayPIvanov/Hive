using Hive.Gig.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Gig.Infrastructure.Persistence.Configurations
{
    public class SellerConfiguration : IEntityTypeConfiguration<Seller>
    {
        public void Configure(EntityTypeBuilder<Seller> builder)
        {
            builder.HasMany(s => s.Gigs)
                .WithOne(g => g.Seller)
                .HasForeignKey(g => g.SellerId);
        }
    }
}