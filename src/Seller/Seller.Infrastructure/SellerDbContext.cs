using System.Reflection;
using Hive.Seller.Domain;
using Microsoft.EntityFrameworkCore;

namespace Hive.Seller.Infrastructure
{
    public class SellerDbContext : DbContext
    {
        public SellerDbContext(DbContextOptions<SellerDbContext> options) : base(options)
        { }

        public DbSet<Gig> Gigs { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Package> Packages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}