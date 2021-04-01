namespace Hive.UserProfile.Domain
{
    using Hive.Common.Domain;
    
    using System.Collections.Generic;

    public class UserProfile : AuditableEntity
    {
        private UserProfile()
        {
            Languages = new HashSet<Language>(5);
            Skills = new HashSet<Skill>(5);
        }
        
        // TODO♦
        public UserProfile(string userId, string firstName, string lastName) : this()
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
        }
        
        public int Id { get; set; }
        
        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? Description { get; set; }
        
        public string? Education { get; set; }

        // TODO: Set to readonly
        public ICollection<Skill> Skills { get; private set; }
        
        public ICollection<Language> Languages { get; private set; }

        public void AddSkill(string value)
        {
            Skills.Add(new Skill(value));
        }
    }
}