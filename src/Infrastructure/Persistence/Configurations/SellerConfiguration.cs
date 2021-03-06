using Hive.Domain.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Infrastructure.Persistence.Configurations
{
    public class SellerConfiguration : IEntityTypeConfiguration<Seller>
    {
        public void Configure(EntityTypeBuilder<Seller> builder)
        {
            builder.Property(s => s.IsDraft).HasDefaultValue(true);

            builder.HasOne(s => s.UserProfile)
                .WithOne(up => up.Seller)
                .HasForeignKey<Seller>(s => s.UserProfileId);

            builder.HasMany(s => s.Gigs)
                .WithOne(g => g.Seller)
                .HasForeignKey(g => g.SellerId);

        }
    }
}