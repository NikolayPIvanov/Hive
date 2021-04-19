using System.Threading;
using System.Threading.Tasks;
using Hive.Gig.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Questions.Interfaces
{
    using Domain.Entities;
    
    public interface IGigManagementDbContext
    {
        DbSet<Category> Categories { get; set; }
        
        DbSet<Gig> Gigs { get; set; }
        
        DbSet<Package> Packages { get; set; }
        
        DbSet<Seller> Sellers { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}