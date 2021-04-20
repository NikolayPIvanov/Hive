using Hive.Common.Core.SeedWork;

namespace Hive.Billing.Domain.Entities
{
    public class AccountHolder : Entity
    {
        private AccountHolder()
        {
        }
        
        public AccountHolder(string userId) : this()
        {
            UserId = userId;
            Account = new Account();
        }
        
        public string UserId { get; private set; }

        public Account Account { get; private set; }

        public int AccountId => Account.Id;
    }
}