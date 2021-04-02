using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using Hive.UserProfile.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.UserProfile.Application.UserProfiles.Queries
{
    public record GetUserProfileByUserIdQuery(string UserId) : IRequest<UserProfileDto>;
    
    public class GetUserProfileByUserIdQueryHandler : IRequestHandler<GetUserProfileByUserIdQuery, UserProfileDto>
    {
        private readonly IUserProfileContext _context;
        private readonly IMapper _mapper;

        public GetUserProfileByUserIdQueryHandler(IUserProfileContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<UserProfileDto> Handle(GetUserProfileByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

            if (userProfile is null)
            {
                throw new NotFoundException(nameof(Domain.UserProfile));
            }

            return _mapper.Map<UserProfileDto>(userProfile);
        }
    }
}