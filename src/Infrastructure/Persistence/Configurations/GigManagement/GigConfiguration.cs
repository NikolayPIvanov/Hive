using Hive.Domain.Entities.Gigs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Infrastructure.Persistence.Configurations.GigManagement
{
    public class GigConfiguration : IEntityTypeConfiguration<Gig>
    {
        public void Configure(EntityTypeBuilder<Gig> builder)
        {
            builder.HasKey(g => g.Id);
            builder.Property(g => g.Title).HasMaxLength(50).IsRequired();
            builder.Property(g => g.IsDraft).IsRequired();

            builder.HasOne(g => g.Category)
                .WithMany()
                .HasForeignKey(g => g.CategoryId);
            
            // https://docs.microsoft.com/en-us/ef/core/modeling/owned-entities#collections-of-owned-types
            builder.OwnsMany<Tag>(g => g.Tags, a =>
            {
                a.WithOwner().HasForeignKey("GigId");
                a.Property<int>("Id");
                a.HasKey("Id");
            });
            
            builder.OwnsMany(g => g.Questions, t =>
            {
                t.WithOwner().HasForeignKey("GigId");
                t.Property<int>("Id");
                t.HasKey("Id");
            });
            
            builder.OwnsOne(g => g.GigScope, t =>
            {
                t.ToTable("scopes");
                t.WithOwner().HasForeignKey("GigId");
                t.Property<int>("Id");
                t.HasKey("Id");
            });

            builder.HasMany(g => g.Packages)
                .WithOne()
                .HasForeignKey(g => g.GigId);
            
            builder.HasMany(g => g.Reviews)
                .WithOne()
                .HasForeignKey(g => g.GigId);
        }
    }
}