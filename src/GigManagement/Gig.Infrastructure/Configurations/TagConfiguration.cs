using Hive.Gig.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Gig.Infrastructure.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.Property(t => t.Value).HasMaxLength(50).IsRequired();
            builder.HasMany(t => t.Gigs)
                .WithMany(g => g.Tags);
        }
    }
}