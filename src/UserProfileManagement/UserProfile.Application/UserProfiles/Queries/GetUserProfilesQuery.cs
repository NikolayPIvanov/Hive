using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Mappings;
using Hive.UserProfile.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.UserProfile.Application.UserProfiles.Queries
{
    public class GetUserProfilesQuery : IRequest<ICollection<UserProfileDto>>
    {
    }
    
    public class GetUserProfilesQueryHandler : IRequestHandler <GetUserProfilesQuery, ICollection<UserProfileDto>>
    {
        private readonly IUserProfileDbContext _context;
        private readonly IMapper _mapper;

        public GetUserProfilesQueryHandler(IUserProfileDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ICollection<UserProfileDto>> Handle(GetUserProfilesQuery request, CancellationToken cancellationToken)
        {
            return await _context.UserProfiles.ProjectToListAsync<UserProfileDto>(_mapper.ConfigurationProvider);
        }
    }
}