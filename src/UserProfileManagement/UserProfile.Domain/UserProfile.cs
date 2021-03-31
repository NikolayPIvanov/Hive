using System.Collections.Immutable;

namespace Hive.UserProfile.Domain
{
    using Hive.Common.Domain;
    
    using System.Collections.Generic;

    public class UserProfile : AuditableEntity
    {
        private HashSet<Skill> _skills;
        private HashSet<Language> _languages;

        private UserProfile()
        {
            _languages = new HashSet<Language>(5);
            _skills = new HashSet<Skill>(5);
        }
        
        // TODO♦
        public UserProfile(string userId, string firstName, string lastName) : this()
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
        }
        
        public int Id { get; set; }
        
        public string UserId { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? Description { get; set; }
        
        public string? Education { get; set; }

        public IReadOnlyCollection<Skill> Skills => _skills.ToImmutableHashSet();

        public IReadOnlyCollection<Language> Languages => _languages.ToImmutableHashSet();

        public void AddSkill(string value)
        {
            _skills.Add(new Skill(value));
        }
    }
}