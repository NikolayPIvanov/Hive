using Hive.Domain.Common;

namespace Hive.Domain.Entities.Accounts
{
    public class UserProfile : AuditableEntity
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Description { get; set; }

        public string? Languages { get; set; }

        public string? Skills { get; set; }

        public string? Education { get; set; }

        public string UserId { get; set; }
    }
}