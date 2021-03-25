using System;
using System.Collections;
using System.Collections.Generic;
using Hive.Common.Domain;

namespace Billing.Domain
{
    public class Account : AuditableEntity
    {
        private Account()
        {
            Transactions = new HashSet<Transaction>();
            PaymentMethods = new HashSet<PaymentMethod>();
        }
        
        public Account(string userId) : this()
        {
            UserId = userId;
        }
        
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
        
        public ICollection<PaymentMethod> PaymentMethods { get; set; }
    }
}