using System.Collections.Generic;
using Hive.Domain.Common;

namespace Hive.Domain.Entities.Billing
{
    public class Wallet : AuditableEntity
    {
        public Wallet()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int AccountHolderId { get; set; }
        
        public ICollection<Transaction> Transactions { get; private set; }
        
    }
}