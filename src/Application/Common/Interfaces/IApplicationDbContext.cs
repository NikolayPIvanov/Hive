using Hive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Hive.Domain.Entities.Accounts;
using Hive.Domain.Entities.Categories;
using Hive.Domain.Entities.Gigs;

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
        

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
