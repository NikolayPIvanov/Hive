using Hive.Common.Core.SeedWork;

namespace Hive.Billing.Domain.Entities
{
    public class AccountHolder : Entity
    {
        private AccountHolder() { }
        public AccountHolder(string userId) : this()
        {
            UserId = userId;
            Wallet = Wallet.CreateEmpty();
        }
        
        public string UserId { get; private set; }

        public Wallet Wallet { get; private set; }

        public int WalletId { get; set; }
    }
}