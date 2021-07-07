using System.Collections.Generic;

namespace Hive.UserProfile.Application.UserProfiles.Queries
{
    public record NotificationSettingDto(bool EmailNotifications = true);
        
    public record UserProfileDto
    {
        public int Id { get; init; }
        
        public string UserId { get; init; }

        public string GivenName { get; init; }

        public string Surname { get; init; }

        public string? Bio { get; init; }
        
        public string? Education { get; init; }
        
        public string? AvatarUri { get; init; }
        
        public ICollection<string> Skills { get;  init; }
        
        public ICollection<string> Languages { get; init; }
    }
}