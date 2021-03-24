using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;

namespace Ordering.Application.Interfaces
{
    public interface IOrderingContext
    {
        public string Schema { get; }
        DbSet<Order> Orders { get; set; }
        
        DbSet<OrderStatus> OrderStatus { get; set; }

        DbSet<Requirement> Requirements { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}