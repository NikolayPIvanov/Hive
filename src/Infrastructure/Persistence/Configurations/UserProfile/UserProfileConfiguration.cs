using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Infrastructure.Persistence.Configurations.UserProfile
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<Domain.Entities.Accounts.UserProfile>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Accounts.UserProfile> builder)
        {
            builder.Property(x => x.FirstName).HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.LastName).HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.Description).HasMaxLength(500).IsRequired(false);
            builder.Property(x => x.Education).HasMaxLength(100).IsRequired(false);

            builder.OwnsMany(g => g.Skills, s =>
            {
                s.WithOwner().HasForeignKey("ProfileId");
                s.Property<int>("Id");
                s.HasKey("Id");
            });
            
            builder.OwnsMany(g => g.Languages, l =>
            {
                l.WithOwner().HasForeignKey("ProfileId");
                l.Property<int>("Id");
                l.HasKey("Id");
            });
        }
    }
}