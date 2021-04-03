using System.Threading;
using System.Threading.Tasks;
using Hive.Investing.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hive.Investing.Application.Interfaces
{
    public interface IInvestingDbContext
    {
        public DbSet<Investor> Investors { get; set; }

        public DbSet<Investment> Investments { get; set; }

        public DbSet<Plan> Plans { get; set; }

        public DbSet<Vendor> Vendors { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());
    }
}