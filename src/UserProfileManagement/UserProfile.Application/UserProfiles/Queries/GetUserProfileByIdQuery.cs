using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using Hive.UserProfile.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hive.UserProfile.Application.UserProfiles.Queries
{
    public record GetUserProfileByIdQuery(int UserProfileId) : IRequest<UserProfileDto>;

    public class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileByIdQuery, UserProfileDto>
    {
        private readonly IUserProfileDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GetUserProfileByIdQueryHandler> _logger;

        public GetUserProfileByIdQueryHandler(IUserProfileDbContext dbContext, IMapper mapper, ILogger<GetUserProfileByIdQueryHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<UserProfileDto> Handle(GetUserProfileByIdQuery request, CancellationToken cancellationToken)
        {
            var userProfile = await _dbContext.UserProfiles.FindAsync(new[] {request.UserProfileId}, cancellationToken);

            if (userProfile is null)
            {
                _logger.LogWarning("User Profile with id: {@Id} was not found.", request.UserProfileId);
                throw new NotFoundException(nameof(Domain.Entities.UserProfile), request.UserProfileId);
            }

            return _mapper.Map<UserProfileDto>(userProfile);
        }
    }
}