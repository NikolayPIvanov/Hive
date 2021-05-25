using Hive.Common.Core.SeedWork;
using Hive.Identity.Contracts;

namespace Hive.Identity.Models
{
    public class UserAccountType : Entity
    {
        private UserAccountType() { }
        public UserAccountType(IdentityType type, string userId) : this()
        {
            Type = type;
            UserId = userId;
        }
        
        public IdentityType Type { get; private init; }
        public string UserId { get; private init; }
    }
}