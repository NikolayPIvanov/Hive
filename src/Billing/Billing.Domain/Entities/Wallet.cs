using System.Collections.Generic;
using Hive.Common.Core.SeedWork;

namespace Hive.Billing.Domain.Entities
{
    public class Wallet : Entity
    {
        public Wallet()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int AccountHolderId { get; set; }
        
        public ICollection<Transaction> Transactions { get; private set; }
        
    }
}