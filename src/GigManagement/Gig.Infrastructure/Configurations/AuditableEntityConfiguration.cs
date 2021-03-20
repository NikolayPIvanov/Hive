using Hive.Common.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Gig.Infrastructure.Configurations
{
    public static class AuditableEntityConfiguration
    {
        public static void ConfigureAuditableEntity<T>(EntityTypeBuilder<T> builder) where T : AuditableEntity
        {
            builder.Property(x => x.Created).HasColumnType("datetime2(2)").IsRequired();
            builder.Property(x => x.LastModified).HasColumnType("datetime2(2)");
            builder.Property(x => x.CreatedBy).HasColumnType("nvarchar(36)");
            builder.Property(x => x.LastModifiedBy).HasColumnType("nvarchar(36)");
        }
    }
}