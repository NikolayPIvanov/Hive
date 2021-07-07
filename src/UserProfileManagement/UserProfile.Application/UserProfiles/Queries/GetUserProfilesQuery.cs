using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hive.Common.Core.Mappings;
using Hive.Common.Core.Models;
using Hive.UserProfile.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.UserProfile.Application.UserProfiles.Queries
{
    public class GetUserProfilesQuery : IRequest<PaginatedList<UserProfileDto>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    
    public class GetUserProfilesQueryHandler : IRequestHandler <GetUserProfilesQuery, PaginatedList<UserProfileDto>>
    {
        private readonly IUserProfileDbContext _context;
        private readonly IMapper _mapper;

        public GetUserProfilesQueryHandler(IUserProfileDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<UserProfileDto>> Handle(GetUserProfilesQuery request, CancellationToken cancellationToken)
        {
            return await _context.UserProfiles
                .ProjectTo<UserProfileDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageIndex, request.PageSize);
        }
    }
}