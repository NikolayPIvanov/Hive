using System.Collections.Generic;
using Hive.Common.Core.SeedWork;

namespace Hive.UserProfile.Domain.Entities
{
    public class UserProfile : Entity
    {
        private UserProfile()
        {
            Languages = new HashSet<Language>(5);
            Skills = new HashSet<Skill>(5);
        }
        
        public UserProfile(string userId, string givenName, string surname) : this()
        {
            UserId = userId;
            GivenName = givenName;
            Surname = surname;
        }

        public string UserId { get; private init; }
        
        public string GivenName { get; set; }

        public string Surname { get; set; }
        
        public string? Bio { get; set; }
        
        public string? AvatarUri { get; set; }
        
        public string? Education { get; set; }
        
        public ICollection<Skill> Skills { get; private set; }
        
        public ICollection<Language> Languages { get; private set; }
        
    }
}