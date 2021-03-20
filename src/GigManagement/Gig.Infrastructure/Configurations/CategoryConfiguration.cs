﻿using Hive.Common.Domain;
using Hive.Gig.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Gig.Infrastructure.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            AuditableEntityConfiguration.ConfigureAuditableEntity(builder);
            builder.Property(c => c.Title).HasMaxLength(50).IsRequired();

            builder.HasMany(c => c.SubCategories)
                .WithOne()
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        
    }
}