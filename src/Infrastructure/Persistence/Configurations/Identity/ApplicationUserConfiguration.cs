using Hive.Domain.Entities.Billing;
using Hive.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Infrastructure.Persistence.Configurations.Identity
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasOne(u => u.Profile)
                .WithOne()
                .HasForeignKey<ApplicationUser>(u => u.UserProfileId);

            builder.HasOne(u => u.AccountHolder)
                .WithOne()
                .HasForeignKey<AccountHolder>(u => u.UserId);
        }
    }
}