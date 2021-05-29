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
        
        public UserProfile(string userId) : this()
        {
            UserId = userId;
            NotificationSetting = new NotificationSetting();
        }

        public string UserId { get; private init; }

        public string? AvatarFile { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Description { get; set; }
        
        public string? Education { get; set; }
        
        public NotificationSetting NotificationSetting { get; set; }

        public ICollection<Skill> Skills { get; private set; }
        
        public ICollection<Language> Languages { get; private set; }
    }
}