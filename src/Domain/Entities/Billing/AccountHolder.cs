using Hive.Domain.Common;

namespace Hive.Domain.Entities.Billing
{
    public class AccountHolder : AuditableEntity
    {
        private AccountHolder()
        {
        }
        
        public AccountHolder(string userId) : this()
        {
            UserId = userId;
            Wallet = new Wallet();
        }
        
        public string UserId { get; private set; }

        public Wallet Wallet { get; private set; }

        public int WalletId { get; set; }
    }
}