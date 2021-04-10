using System;
using Hive.Domain.Common;
using Hive.Domain.Enums;

namespace Hive.Domain.Entities.Billing
{
    public class Transaction : AuditableEntity
    {
        private Transaction()
        {
        }
        
        public Transaction(decimal amount, Guid? orderNumber, TransactionType type, int walletId) : this()
        {
            Amount = amount;
            OrderNumber = orderNumber;
            TransactionType = type;
            WalletId = walletId;
            PublicId = GenerateTransactionId();
        }
        
        public int PublicId { get; init; }
        public decimal Amount { get; private init; }
        
        public TransactionType TransactionType { get; private init; }

        public Guid? OrderNumber { get; init; }
        
        public int WalletId { get; private init; }
        
        public Wallet Wallet { get; private init; }


        private int GenerateTransactionId()
        {
            Random jitter = new ((int)DateTime.Now.Ticks);
            var id = jitter.Next(1, 100000000);
            return id;
        }
    }
}