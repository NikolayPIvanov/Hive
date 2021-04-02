using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Gig.Infrastructure.Persistence.Configurations
{
    public class GigConfiguration : IEntityTypeConfiguration<Domain.Entities.Gig>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Gig> builder)
        {
            
            builder.HasKey(g => g.Id);
            builder.Property(g => g.Title).HasMaxLength(50).IsRequired();
            builder.Property(g => g.IsDraft).IsRequired();

            builder.HasOne(g => g.Category)
                .WithMany()
                .HasForeignKey(g => g.CategoryId);
            
            // https://docs.microsoft.com/en-us/ef/core/modeling/owned-entities#collections-of-owned-types
            builder.OwnsMany(g => g.Tags, t =>
            {
                t.WithOwner().HasForeignKey("GigId");
                t.Property<int>("Id");
                t.HasKey("Id");
            });

            builder.HasMany(g => g.Packages)
                .WithOne()
                .HasForeignKey(g => g.GigId);

            builder.HasMany(g => g.Questions)
                .WithOne()
                .HasForeignKey(q => q.GigId);
            
            builder.HasOne(g => g.GigScope)
                .WithOne()
                .HasForeignKey<Domain.Entities.Gig>(gs => gs.GigScopeId);
        }
    }
}