using Billing.Domain;
using Microsoft.EntityFrameworkCore;

namespace Billing.Application.Interfaces
{
    public interface IBillingContext
    {
        public DbSet<Account> Accounts { get; set; }
        
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }
    }
}