using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Accounts;
using MediatR;

namespace Hive.Application.UserProfiles.Queries
{
    public record UserProfileDto();

    public record GetUserProfileByIdQuery(int Id) : IRequest<UserProfileDto>;

    public class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileByIdQuery, UserProfileDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetUserProfileByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<UserProfileDto> Handle(GetUserProfileByIdQuery request, CancellationToken cancellationToken)
        {
            var profile = await _context.Profiles.FindAsync(request.Id);

            if (profile == null)
            {
                throw new NotFoundException(nameof(UserProfile), request.Id);
            }

            return _mapper.Map<UserProfileDto>(profile);
        }
    }
}