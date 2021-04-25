using AutoMapper;
using Hive.UserProfile.Application.UserProfiles.Queries;

namespace Hive.UserProfile.Application.UserProfiles
{
    using Domain.Entities;
    
    public class UserProfileProfile : Profile
    {
        public UserProfileProfile()
        {
            CreateMap<UserProfile, UserProfileDto>()
                .ForMember(d => d.IsTransient,
                    x => x.Condition(
                        profile => !string.IsNullOrEmpty(profile.FirstName) && !string.IsNullOrEmpty(profile.LastName)));

            CreateMap<NotificationSetting, NotificationSettingDto>().DisableCtorValidation().ReverseMap();
        }
    }
}