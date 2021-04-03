using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using Hive.UserProfile.Application.Interfaces;
using MediatR;

namespace Hive.UserProfile.Application.UserProfiles.Queries
{
    public record GetUserProfileQuery(int UserProfileId) : IRequest<UserProfileDto>;

    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto>
    {
        private readonly IUserProfileDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetUserProfileQueryHandler(IUserProfileDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        public async Task<UserProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var userProfile = await _dbContext.UserProfiles.FindAsync(new[] {request.UserProfileId}, cancellationToken);

            if (userProfile is null)
            {
                throw new NotFoundException(nameof(Domain.UserProfile), request.UserProfileId);
            }

            return _mapper.Map<UserProfileDto>(userProfile);
        }
    }
}