using Hive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Hive.Domain.Entities.Accounts;
using Hive.Domain.Entities.Categories;
using Hive.Domain.Entities.Gigs;
using Hive.Domain.Entities.Investments;
using Hive.Domain.Entities.Orders;

namespace Hive.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Category> Categories { get; set; }
        
        DbSet<Gig> Gigs { get; set; }

        DbSet<Seller> Sellers { get; set; }
        
        DbSet<UserProfile> Profiles { get; set; }
        
        DbSet<Package> Packages { get; set; }
        
        DbSet<Order> Orders { get; set; }
        
        DbSet<Requirement> Requirements { get; set; }
        
        DbSet<Review> Reviews { get; set; }
        
        DbSet<Plan> Plans { get; set; }
        DbSet<Investor> Investors { get; set; }
        DbSet<Investment> Investments { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
