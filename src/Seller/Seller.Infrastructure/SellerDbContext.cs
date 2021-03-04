using System.Reflection;
using Hive.Domain.Entities;
using Hive.Domain.Entities.Gigs;
using Microsoft.EntityFrameworkCore;

namespace Hive.Seller.Infrastructure
{
    public class SellerDbContext : DbContext
    {
        private const string SchemaName = "seller";
        
        public SellerDbContext(DbContextOptions<SellerDbContext> options) : base(options)
        { }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.HasDefaultSchema(SchemaName);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}