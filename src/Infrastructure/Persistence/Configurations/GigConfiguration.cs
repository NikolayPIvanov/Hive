using Hive.Domain.Entities.Gigs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Infrastructure.Persistence.Configurations
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
            
            builder.HasOne(g => g.Seller)
                .WithMany()
                .HasForeignKey(g => g.SellerId);
            
            // https://docs.microsoft.com/en-us/ef/core/modeling/owned-entities#collections-of-owned-types
            builder.OwnsMany(g => g.Tags, t =>
            {
                t.WithOwner().HasForeignKey("GigId");
                t.Property<int>("Id");
                t.HasKey("Id");
            });
            
            builder.OwnsMany(g => g.Questions, t =>
            {
                t.WithOwner().HasForeignKey("GigId");
                t.Property<int>("Id");
                t.HasKey("Id");
            });
            
            builder.OwnsOne(g => g.GigScope, t =>
            {
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
    
    public class PackageConfiguration : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.Property(x => x.Title)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.DeliveryTime)
                .IsRequired();

            builder.Property(x => x.Revisions)
                .IsRequired();
        }
    }
    
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            
            builder.Property(x => x.Title)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Answer)
                .HasMaxLength(1000)
                .IsRequired();
        }
    }
    
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.Property(x => x.Comment).HasMaxLength(1000).IsRequired();
            builder.Property(x => x.Rating).IsRequired();

            builder.HasCheckConstraint("CK_Review_Rating", "[Rating] BETWEEN 1.0 AND 5.0");
        }
    }
    
    public class SellerConfiguration : IEntityTypeConfiguration<Seller>
    {
        public void Configure(EntityTypeBuilder<Seller> builder)
        {
            // builder.HasMany(s => s.Gigs)
            //     .WithOne(g => g.Seller)
            //     .HasForeignKey(g => g.SellerId);
        }
    }
}