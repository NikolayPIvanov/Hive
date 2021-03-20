using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Gig.Infrastructure.Configurations
{
    public class GigConfiguration : IEntityTypeConfiguration<Domain.Entities.Gig>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Gig> builder)
        {
            AuditableEntityConfiguration.ConfigureAuditableEntity(builder);
            
            builder.HasKey(g => g.Id);
            builder.Property(g => g.Title).HasMaxLength(50).IsRequired();

            builder.HasOne(g => g.Category)
                .WithMany()
                .HasForeignKey(g => g.CategoryId);

            builder.HasMany(g => g.Tags)
                .WithMany(t => t.Gigs);
        }
    }
}