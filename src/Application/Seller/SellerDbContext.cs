using System.Reflection;
using Hive.Seller.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hive.Seller
{
    public class GigConfiguration : IEntityTypeConfiguration<Gig>
    {
        public void Configure(EntityTypeBuilder<Gig> builder)
        {
            builder.HasOne(go => go.GigOverview)
                .WithOne(go => go.Gig)
                .HasForeignKey<GigOverview>(x => x.GigId);

            builder.HasOne(go => go.Seller)
                .WithMany(s => s.Gigs)
                .HasForeignKey(x => x.SellerId);
        }
    }
    public class SellerDbContext : DbContext
    {
        public DbSet<Domain.Seller> Sellers { get; set; }
        public DbSet<Gig> Gigs { get; set; }
        public DbSet<GigOverview> GigOverviews { get; set; }
        public DbSet<Category> Categories { get; set; }
        
        public SellerDbContext(DbContextOptions<SellerDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GigConfiguration).Assembly);
        }
    }
}