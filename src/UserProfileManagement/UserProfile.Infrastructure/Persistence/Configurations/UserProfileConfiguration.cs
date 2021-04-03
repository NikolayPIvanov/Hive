using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.UserProfile.Infrastructure.Persistence.Configurations
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<Domain.UserProfile>
    {
        public void Configure(EntityTypeBuilder<Domain.UserProfile> builder)
        {
            builder.ToTable("profiles", UserProfileDbContext.Schema);
            builder.HasKey(p => p.Id);

            builder.Property(p => p.FirstName).HasMaxLength(50).IsRequired();
            builder.Property(p => p.LastName).HasMaxLength(50).IsRequired();
            builder.Property(p => p.Education).HasMaxLength(100).IsRequired(false);
            builder.Property(p => p.Description).HasMaxLength(50).IsRequired(false);

            // https://docs.microsoft.com/en-us/ef/core/modeling/owned-entities#collections-of-owned-types
            builder.OwnsMany(p => p.Languages, l =>
            {
                l.WithOwner().HasForeignKey("ProfileId");
                l.Property<int>("Id");
                l.HasKey("Id");
            });
            
            builder.OwnsMany(p => p.Skills, l =>
            {
                l.WithOwner().HasForeignKey("ProfileId");
                l.Property<int>("Id");
                l.HasKey("Id");
            });
        }
    }
}