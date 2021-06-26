namespace Hive.UserProfile.Infrastructure.Persistence.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Domain.Entities;
    
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.UserProfile> builder)
        {
            builder.ToTable("Profiles", UserProfileDbContext.Schema);
            builder.HasKey(p => p.Id);

            builder.Property(p => p.GivenName).HasMaxLength(50).IsRequired();
            builder.Property(p => p.Surname).HasMaxLength(50).IsRequired();
            
            builder.Property(p => p.Education).HasMaxLength(100).IsRequired(false);
            builder.Property(p => p.Bio).HasMaxLength(2500).IsRequired(false);

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