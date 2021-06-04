using System.Linq;
using AutoMapper;
using Hive.UserProfile.Application.UserProfiles.Queries;

namespace Hive.UserProfile.Application.UserProfiles
{
    using Domain.Entities;
    
    public class UserProfileMapping : Profile
    {
        public UserProfileMapping()
        {
            CreateMap<UserProfile, UserProfileDto>()
                .ForMember(d => d.IsTransient,
                    x => x.Condition(
                        profile => !string.IsNullOrEmpty(profile.FirstName) && !string.IsNullOrEmpty(profile.LastName)))
                .ForMember(d => d.Languages, x => x.MapFrom(s => s.Languages.Select(l => l.Value)))
                .ForMember(d => d.Skills, x => x.MapFrom(s => s.Skills.Select(l => l.Value)))
                .ForMember(d => d.NotificationSettings, x => x.MapFrom(s => s.NotificationSetting));
            CreateMap<NotificationSetting, NotificationSettingDto>().DisableCtorValidation().ReverseMap();
        }
    }
}