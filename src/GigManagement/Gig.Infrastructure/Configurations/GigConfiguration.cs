using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Gig.Infrastructure.Configurations
{
    public class GigConfiguration : IEntityTypeConfiguration<Domain.Entities.Gig>
    {
        // TODO: Add column types
        public void Configure(EntityTypeBuilder<Domain.Entities.Gig> builder)
        {
            AuditableEntityConfiguration.ConfigureAuditableEntity(builder);
            
            builder.HasKey(g => g.Id);
            builder.Property(g => g.Title).HasMaxLength(50).IsRequired();
            builder.Property(g => g.Description).HasMaxLength(1000).IsRequired();

            // builder.Property(g => g.Metadata).HasMaxLength(50);
            // builder.Property(g => g.Tags).HasMaxLength(100).IsRequired();

            // builder.HasOne(g => g.Category)
            //     .WithOne()
            //     .HasForeignKey<Gig>(g => g.CategoryId);
            //
            // builder.HasMany(g => g.Orders)
            //     .WithOne()
            //     .HasForeignKey(x => x.GigId)
            //     .OnDelete(DeleteBehavior.NoAction);
            //
            // builder.HasMany(g => g.Questions)
            //     .WithOne()
            //     .HasForeignKey(g => g.GigId);
            //
            // builder.HasMany(g => g.Reviews)
            //     .WithOne()
            //     .HasForeignKey(r => r.GigId);
        }
    }
    
    // public class GigQuestionConfiguration : IEntityTypeConfiguration<GigQuestion>
    // {
    //     public void Configure(EntityTypeBuilder<GigQuestion> builder)
    //     {
    //         builder.Property(g => g.Question).HasMaxLength(50).IsRequired();
    //         builder.Property(g => g.Answer).HasMaxLength(50).IsRequired();
    //     }
    // }
}