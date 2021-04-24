using Hive.Common.Core.SeedWork;

namespace Hive.Identity.Models
{
    public class UserAccountType : Entity
    {
        public AccountType Type { get; set; }
        public string UserId { get; set; }
    }
}