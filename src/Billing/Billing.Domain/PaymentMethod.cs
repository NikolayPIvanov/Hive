using Hive.Common.Domain;

namespace Billing.Domain
{
    // TODO: Not used in business logic
    public class PaymentMethod : AuditableEntity
    {
        private PaymentMethod()
        {
        }

        public PaymentMethod(string type, int accountId)
        {
            Type = type;
            AccountId = accountId;
        }
        
        public int Id { get; set; }

        public string Type { get; set; }

        public int AccountId { get; set; }
        
        public Account Account { get; set; }
    }
}