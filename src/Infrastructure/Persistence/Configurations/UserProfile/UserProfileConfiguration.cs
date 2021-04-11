using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Infrastructure.Persistence.Configurations.UserProfile
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<Domain.Entities.Accounts.UserProfile>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Accounts.UserProfile> builder)
        {
            // TODO: Configuration
        }
    }
}