using Hive.Domain.Entities.Gigs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Infrastructure.Persistence.Configurations
{
    public class GigConfiguration : IEntityTypeConfiguration<Gig>
    {
        // TODO: Add column types
        public void Configure(EntityTypeBuilder<Gig> builder)
        {
            builder.Property(g => g.Title).HasMaxLength(50).IsRequired();
            builder.Property(g => g.Metadata).HasMaxLength(50);
            builder.Property(g => g.Tags).HasMaxLength(100).IsRequired();
            builder.Property(g => g.Description).HasMaxLength(255).IsRequired();

            builder.HasOne(g => g.Category)
                .WithOne()
                .HasForeignKey<Gig>(g => g.CategoryId);

            builder.HasMany(g => g.Orders)
                .WithOne()
                .HasForeignKey(x => x.GigId);
            
            builder.HasMany(g => g.Packages)
                .WithOne(p => p.Gig)
                .HasForeignKey(g => g.GigId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(g => g.Questions)
                .WithOne()
                .HasForeignKey(g => g.GigId);
        }
    }
    
    public class GigQuestionConfiguration : IEntityTypeConfiguration<GigQuestion>
    {
        public void Configure(EntityTypeBuilder<GigQuestion> builder)
        {
            builder.Property(g => g.Question).HasMaxLength(50).IsRequired();
            builder.Property(g => g.Answer).HasMaxLength(50).IsRequired();
        }
    }
}