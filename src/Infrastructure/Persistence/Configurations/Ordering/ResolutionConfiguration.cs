﻿using Hive.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Infrastructure.Persistence.Configurations.Ordering
{
    public class ResolutionConfiguration : IEntityTypeConfiguration<Resolution>
    {
        public void Configure(EntityTypeBuilder<Resolution> builder)
        {
            builder.Property(x => x.Version).IsRequired();
            builder.Property(x => x.Location).IsRequired();
            builder.Property(x => x.IsApproved).IsRequired();

            builder.HasOne(x => x.Order)
                .WithMany(o => o.Resolutions)
                .HasForeignKey(x => x.OrderId);
        }
    }
}