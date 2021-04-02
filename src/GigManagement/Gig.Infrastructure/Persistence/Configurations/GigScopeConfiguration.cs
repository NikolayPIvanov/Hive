using Hive.Gig.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Gig.Infrastructure.Persistence.Configurations
{
    public class GigScopeConfiguration : IEntityTypeConfiguration<GigScope>
    {
        public void Configure(EntityTypeBuilder<GigScope> builder)
        {

            builder.Property(gs => gs.Description)
                .HasMaxLength(2000)
                .IsRequired();

            builder.HasIndex(gs => gs.GigId).IsUnique();
        }
    }
}