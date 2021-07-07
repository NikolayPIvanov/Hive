using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Interfaces
{
    public interface IOrderingContext
    {
        DbSet<Order> Orders { get; set; }
        DbSet<Resolution> Resolutions { get; set; }
        DbSet<Buyer> Buyers { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}