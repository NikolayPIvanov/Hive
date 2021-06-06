using Hive.Common.Core.SeedWork;

namespace Hive.Billing.Domain.Entities
{
    public class AccountHolder : Entity
    {
        private AccountHolder() { }
        public AccountHolder(string userId) : this()
        {
            UserId = userId;
        }
        
        public string UserId { get; private set; }
    }
}