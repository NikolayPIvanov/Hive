using System.Collections.Generic;
using Hive.Domain.Common;

namespace Hive.Domain.Entities.Accounts
{
    public record Skill(string Value);
    public record Language(string Value);
    public class UserProfile : AuditableEntity
    {
        public UserProfile()
        {
            Languages = new HashSet<Language>(5);
            Skills = new HashSet<Skill>(5);
        }
        
        public UserProfile(string userId) : this()
        {
            UserId = userId;
        }

        public string UserId { get; private init; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Description { get; set; }
        
        public string? Education { get; set; }

        public ICollection<Skill> Skills { get; set; }
        
        public ICollection<Language> Languages { get; set; }
    }
}