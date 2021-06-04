using System.Collections.Generic;

namespace Hive.UserProfile.Application.UserProfiles.Queries
{
    public record NotificationSettingDto(bool EmailNotifications = true);
        
    public record UserProfileDto
    {
        public int Id { get; init; }
        
        public string UserId { get; init; }

        public string? FirstName { get; init; }

        public string? LastName { get; init; }

        public string? Description { get; init; }
        
        public string? Education { get; init; }
        
        public string? AvatarFile { get; init; }
        
        public NotificationSettingDto NotificationSettings { get; init; }

        public bool IsTransient { get; init; }

        public ICollection<string> Skills { get;  init; }
        
        public ICollection<string> Languages { get; init; }
    }
}