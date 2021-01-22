using Microsoft.EntityFrameworkCore;

namespace Seller
{
    public class SellerDbContext : DbContext
    {
        public DbSet<Domain.Seller.Seller> Sellers { get; set; }

        public SellerDbContext(DbContextOptions<SellerDbContext> options) : base(options)
        { }
    }
}