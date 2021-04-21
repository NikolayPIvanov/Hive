using Hive.Common.Core.SeedWork;

namespace Hive.Billing.Domain.Entities
{
    public class AccountHolder : Entity
    {
        public AccountHolder()
        {
            Wallet = new Wallet();
        }
        
        public AccountHolder(string userId) : this()
        {
            UserId = userId;
        }
        
        public string UserId { get; private set; }

        public Wallet Wallet { get; private set; }

        public int WalletId { get; set; }
    }
}