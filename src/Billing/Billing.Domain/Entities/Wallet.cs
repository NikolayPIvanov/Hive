using System.Collections.Generic;
using Hive.Billing.Domain.Enums;
using Hive.Common.Core.SeedWork;

namespace Hive.Billing.Domain.Entities
{
    public class Wallet : Entity
    {
        private Wallet()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int AccountHolderId { get; private init; }

        public AccountHolder AccountHolder { get; set; }
        
        public decimal Balance { get; private set; }
        public ICollection<Transaction> Transactions { get; private set; }

        public static Wallet CreateEmpty() => new();

        public void AddTransaction(Transaction transaction)
        {
            var difference = transaction.TransactionType == TransactionType.Fund
                ? transaction.Amount
                : -1.0m * transaction.Amount;
            Balance += difference;
            
            Transactions.Add(transaction);
        }
        
    }
}