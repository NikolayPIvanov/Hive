using System;

namespace Billing.Application.Objects
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        
        public string TransactionType { get; set; }
        
        public decimal Amount { get; set; }
        
        public Guid? OrderNumber { get; set; }
        
        public Guid AccountId { get; set; }
        
        public Guid PaymentMethodId { get; set; }
    }
}