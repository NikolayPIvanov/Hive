using Hive.Common.Domain;
using Hive.Common.Domain.SeedWork;

namespace Billing.Domain
{
    // TODO: Not used in business logic
    public class PaymentMethod : Entity
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