﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence.Configurations
{
    public class BuyerConfiguration : IEntityTypeConfiguration<Buyer>
    {
        public void Configure(EntityTypeBuilder<Buyer> builder)
        {
            builder.Property(b => b.UserId).IsRequired();
            
            builder.HasMany(b => b.Orders)
                .WithOne()
                .HasForeignKey(b => b.BuyerId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}