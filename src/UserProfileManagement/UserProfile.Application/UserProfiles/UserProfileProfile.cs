using AutoMapper;
using Hive.UserProfile.Application.UserProfiles.Queries;

namespace Hive.UserProfile.Application.UserProfiles
{
    public class UserProfileProfile : Profile
    {
        public UserProfileProfile()
        {
            CreateMap<Domain.UserProfile, UserProfileDto>()
                .ForMember(d => d.IsTransient,
                    x => x.Condition(
                        profile => !string.IsNullOrEmpty(profile.FirstName) &&
                                   !string.IsNullOrEmpty(profile.LastName)));
        }
    }
}