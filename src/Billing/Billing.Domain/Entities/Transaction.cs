using System;
using Hive.Billing.Domain.Enums;
using Hive.Common.Core.SeedWork;

namespace Hive.Billing.Domain.Entities
{
    public class Transaction : Entity
    {
        private const int JitterMaxValue = 100000000;

        private Transaction()
        {
        }
        
        public Transaction(decimal amount, Guid? orderNumber, TransactionType type) : this()
        {
            TransactionNumber = GenerateTransactionId();
            Amount = amount;
            OrderNumber = orderNumber;
            TransactionType = type;
        }
        
        public int TransactionNumber { get; private init; }
        
        public decimal Amount { get; private init; }
        
        public TransactionType TransactionType { get; private init; }

        public Guid? OrderNumber { get; private init; }
        
        public int WalletId { get; private init; }
        
        public Wallet Wallet { get; private init; }


        private int GenerateTransactionId()
        {
            Random jitter = new ((int)DateTime.Now.Ticks);
            var id = jitter.Next(1, maxValue: JitterMaxValue);
            return id;
        }
    }
}