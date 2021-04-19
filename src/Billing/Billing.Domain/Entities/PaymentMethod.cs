using System.Collections.Generic;
using Hive.Common.Core.SeedWork;
using Hive.Common.Domain.SeedWork;

namespace Hive.Billing.Domain.Entities
{
    public class PaymentMethod : Entity
    {
        private PaymentMethod()
        {
            Transactions = new HashSet<Transaction>();
        }

        public PaymentMethod(string alias, int accountId) : this()
        {
            Alias = alias;
            AccountId = accountId;
        }
        
        public string Alias { get; set; }

        public int AccountId { get; set; }
        
        public Account Account { get; set; }
        
        public ICollection<Transaction> Transactions { get; private set; }

    }
}