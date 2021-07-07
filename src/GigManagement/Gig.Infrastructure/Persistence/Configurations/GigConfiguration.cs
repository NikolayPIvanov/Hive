using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Gig.Infrastructure.Persistence.Configurations
{
    using Domain.Entities;
    
    public class GigConfiguration : IEntityTypeConfiguration<Gig>
    {
        public void Configure(EntityTypeBuilder<Gig> builder)
        {
            builder.HasKey(g => g.Id);
            builder.Property(g => g.Title).HasMaxLength(100).IsRequired();
            builder.Property(g => g.IsDraft).IsRequired();

            builder.HasOne(g => g.Category)
                .WithMany()
                .HasForeignKey(g => g.CategoryId);
            
            builder.HasMany(g => g.Packages)
                .WithOne(p => p.Gig)
                .HasForeignKey(g => g.GigId);
            
            // https://docs.microsoft.com/en-us/ef/core/modeling/owned-entities#collections-of-owned-types
            builder.OwnsMany(g => g.Tags, t =>
            {
                t.ToTable("GigTags");
                t.WithOwner().HasForeignKey("GigId");
                t.Property<int>("Id");
                t.HasKey("Id");
            });
            
            builder.OwnsMany(g => g.Questions, q =>
            {
                q.ToTable("Questions");
                q.WithOwner().HasForeignKey("GigId");
                q.Property<int>("Id");
                q.HasKey("Id");
                
                q.Property(x => x.Answer)
                    .HasMaxLength(50)
                    .IsRequired();
                
                q.Property(x => x.Answer)
                    .HasMaxLength(1000)
                    .IsRequired();
            });
            
            builder.OwnsMany(g => g.Images, q =>
            {
                q.ToTable("GigsImages");
                q.WithOwner().HasForeignKey("GigId");
                q.Property<int>("Id");
                q.HasKey("Id");
                
                q.Property(x => x.Path).IsRequired();
            });
            
            builder.OwnsOne(g => g.GigScope, q =>
            {
                q.ToTable("Scopes");
                q.WithOwner().HasForeignKey("GigId");
                q.Property<int>("Id");
                q.HasKey("Id");
                
                q.Property(x => x.Description)
                    .HasMaxLength(2500)
                    .IsRequired();
            });
        }
    }
}