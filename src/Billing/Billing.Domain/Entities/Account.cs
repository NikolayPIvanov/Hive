using System.Collections.Generic;
using Hive.Common.Domain.SeedWork;

namespace Hive.Billing.Domain.Entities
{
    public class Account : Entity
    {
        public Account()
        {
            PaymentMethods = new HashSet<PaymentMethod>();
        }

        public int AccountHolderId { get; set; }

        public int? DefaultPaymentMethodId { get; set; }

        public PaymentMethod DefaultPaymentMethod { get; set; }
        
        public ICollection<PaymentMethod> PaymentMethods { get; private set; }
    }
}