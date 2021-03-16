using Hive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Hive.Domain.Entities.Accounts;
using Hive.Domain.Entities.Categories;
using Hive.Domain.Entities.Gigs;
using Hive.Domain.Entities.Orders;

namespace Hive.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<TodoList> TodoLists { get; set; }

        DbSet<TodoItem> TodoItems { get; set; }
        
        DbSet<Category> Categories { get; set; }
        
        DbSet<Gig> Gigs { get; set; }

        DbSet<Seller> Sellers { get; set; }
        
        DbSet<UserProfile> Profiles { get; set; }
        
        DbSet<Package> Packages { get; set; }
        
        DbSet<Order> Orders { get; set; }
        
        DbSet<Requirement> Requirements { get; set; }
        
        DbSet<Review> Reviews { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
