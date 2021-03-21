using System.Reflection;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Infrastructure
{
    public class GigManagementContext : DbContext, IGigManagementContext
    {
        public GigManagementContext(
            DbContextOptions<GigManagementContext> options) : base(options)
        {
        }
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<Domain.Entities.Gig> Gigs { get; set; }
        public DbSet<GigScope> GigScopes { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("gmt");
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}