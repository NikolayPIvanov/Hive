using System.Threading;
using System.Threading.Tasks;
using Hive.Gig.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Interfaces
{
    public interface IGigManagementContext
    {
        DbSet<Category> Categories { get; set; }
        
        DbSet<Domain.Entities.Gig> Gigs { get; set; }
        
        DbSet<GigScope> GigScopes { get; set; }

        DbSet<Package> Packages { get; set; }
        
        DbSet<Tag> Tags { get; set; }
        
        DbSet<Question> Questions { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}