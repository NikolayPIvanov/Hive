using Hive.Common.Core.SeedWork;
using Hive.Identity.Contracts;

namespace Hive.Identity.Models
{
    public class UserAccountType : Entity
    {
        public IdentityType Type { get; set; }
        public string UserId { get; set; }
    }
}