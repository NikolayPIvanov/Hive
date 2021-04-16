using Hive.Domain.Common;

namespace Hive.Domain.Entities.Billing
{
    public class AccountHolder : AuditableEntity
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