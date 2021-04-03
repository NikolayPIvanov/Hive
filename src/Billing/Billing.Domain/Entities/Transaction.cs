using System;
using Hive.Billing.Domain.Enums;
using Hive.Common.Domain.SeedWork;

namespace Hive.Billing.Domain.Entities
{
    public class Transaction : Entity
    {
        private Transaction()
        {
        }
        
        public Transaction(TransactionType type, decimal amount, int paymentMethodId, Guid? orderNumber) : this()
        {
            Amount = amount;
            OrderNumber = orderNumber;
            PaymentMethodId = paymentMethodId;
            TransactionType = type;
            TransactionId = GenerateTransactionId();
        }
        
        public TransactionType TransactionType { get; private init; }

        public int TransactionId { get; init; }

        public decimal Amount { get; private init; }

        public Guid? OrderNumber { get; init; }
        
        public int PaymentMethodId { get; private init; }

        private int GenerateTransactionId()
        {
            Random jitter = new ((int)DateTime.Now.Ticks);
            var id = jitter.Next(1, 100000000);
            return id;
        }
    }
}