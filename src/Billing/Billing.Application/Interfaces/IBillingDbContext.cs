using System.Threading;
using System.Threading.Tasks;
using Hive.Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Billing.Application.Interfaces
{
    public interface IBillingDbContext
    {
        public DbSet<Wallet> Wallets { get; set; }
        
        public DbSet<AccountHolder> AccountHolders { get; set; }
        
        public DbSet<Transaction> Transactions { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        
    }
}