using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Interfaces
{
    using Domain.Entities;
    
    public interface IGigManagementDbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Gig> Gigs { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());

    }
}