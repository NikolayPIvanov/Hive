using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Application.Exceptions;
using Hive.UserProfile.Application.Interfaces;
using MediatR;

namespace Hive.UserProfile.Application.UserProfiles.Queries
{
    public record GetUserProfileQuery(string UserProfileId) : IRequest<UserProfileDto>;

    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto>
    {
        private readonly IUserProfileContext _context;
        private readonly IMapper _mapper;

        public GetUserProfileQueryHandler(IUserProfileContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<UserProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var userProfile = await _context.UserProfiles.FindAsync(new[] {request.UserProfileId}, cancellationToken);

            if (userProfile is null)
            {
                throw new NotFoundException(nameof(Domain.UserProfile), request.UserProfileId);
            }

            return _mapper.Map<UserProfileDto>(userProfile);
        }
    };
}