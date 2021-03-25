using System;
using Hive.Common.Domain;

namespace Billing.Domain
{
    public class PaymentMethod : AuditableEntity
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public Guid AccountId { get; set; }
    }
}