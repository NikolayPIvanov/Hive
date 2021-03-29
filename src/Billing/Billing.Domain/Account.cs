using System;
using System.Collections;
using System.Collections.Generic;
using Hive.Common.Domain;

namespace Billing.Domain
{
    public class AccountHolder : AuditableEntity
    {
        public AccountHolder(string userId)
        {
            UserId = userId;
        }
        
        public int Id { get; set; }
        
        public string UserId { get; set; }

        public Account Account { get; set; }

        public int AccountId { get; set; }
    }
    
    public class Account : AuditableEntity
    {
        private Account()
        {
            Transactions = new HashSet<Transaction>();
            PaymentMethods = new HashSet<PaymentMethod>();
        }
        
        public Account(string userId) : this()
        {
            AccountHolder = new AccountHolder(userId);
        }
        
        public int Id { get; set; }

        public int AccountHolderId { get; set; }
        
        public AccountHolder AccountHolder { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        
        public ICollection<PaymentMethod> PaymentMethods { get; set; }
    }
}