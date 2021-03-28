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

            builder.HasMany(g => g.Tags)
                .WithMany(t => t.Gigs);

            builder.HasMany(g => g.Packages)
                .WithOne(p => p.Gig)
                .HasForeignKey(g => g.GigId);

            builder.HasMany(g => g.Questions)
                .WithOne()
                .HasForeignKey(q => q.GigId);
            
            builder.HasOne(g => g.GigScope)
                .WithOne(gs => gs.Gig)
                .HasForeignKey<Domain.Entities.Gig>(gs => gs.GigScopeId);
        }
    }
}