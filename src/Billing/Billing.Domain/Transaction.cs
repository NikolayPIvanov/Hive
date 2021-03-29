using System;
using Hive.Common.Domain;

namespace Billing.Domain
{
    public class Transaction : AuditableEntity
    {
        private Transaction()
        {
        }
        
        public Transaction(TransactionType type, decimal amount, int accountId, int paymentMethodId, Guid? orderNumber) : this()
        {
            Amount = amount;
            OrderNumber = orderNumber;
            AccountId = accountId;
            PaymentMethodId = paymentMethodId;
            TransactionType = type;
        }
        
        public Guid Id { get; set; }

        public TransactionType TransactionType { get; set; }

        public decimal Amount { get; set; }

        public Guid? OrderNumber { get; set; }

        public int AccountId { get; set; }

        public Account Account { get; set; }
        
        public int PaymentMethodId { get; set; }
    }
}