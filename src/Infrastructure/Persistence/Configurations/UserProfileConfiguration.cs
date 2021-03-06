using Hive.Domain.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Infrastructure.Persistence.Configurations
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.HasOne(up => up.Seller)
                .WithOne(s => s.UserProfile)
                .HasForeignKey<UserProfile>(up => up.SellerId);
        }
    }
}