using Hive.Domain.Entities.Investments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Infrastructure.Persistence.Configurations
{
    public class PlanCategoryConfiguration : IEntityTypeConfiguration<PlanCategory>
    {
        public void Configure(EntityTypeBuilder<PlanCategory> builder)
        {
            builder.HasKey(x => new {x.PlanId, x.CategoryId});
        }
    }
}