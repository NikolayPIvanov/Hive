using Hive.Domain.Entities;
using Hive.Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Infrastructure.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.Title).HasMaxLength(50).IsRequired();

            builder.HasMany(c => c.SubCategories)
                .WithOne()
                .HasForeignKey(c => c.ParentCategoryId);
        }
    }
}