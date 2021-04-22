using Hive.Common.Core.SeedWork;

namespace Hive.UserProfile.Domain
{
    using System.Collections.Generic;
    
    public record Language(string Value);
    
    public record Skill(string Value);
    
    public class UserProfile : Entity
    {
        private UserProfile()
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

        public ICollection<Skill> Skills { get; private set; }
        
        public ICollection<Language> Languages { get; private set; }
    }
}