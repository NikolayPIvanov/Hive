using System;
using Hive.Common.Domain;

namespace Billing.Domain
{
    public class Transaction : AuditableEntity
    {
        public Transaction(decimal amount, Guid orderNumber, Guid accountId, Guid paymentMethodId)
        {
            Amount = amount;
            OrderNumber = orderNumber;
            AccountId = accountId;
            PaymentMethodId = paymentMethodId;
        }
        
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public Guid OrderNumber { get; set; }

        public Guid AccountId { get; set; }

        public Account Account { get; set; }
        
        public Guid PaymentMethodId { get; set; }
    }
}