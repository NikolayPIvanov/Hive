using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using Hive.UserProfile.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.UserProfile.Application.UserProfiles.Queries
{
    public record GetUserProfileByUserIdQuery(string UserId) : IRequest<UserProfileDto>;
    
    public class GetUserProfileByUserIdQueryHandler : IRequestHandler<GetUserProfileByUserIdQuery, UserProfileDto>
    {
        private readonly IUserProfileDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GetUserProfileByUserIdQueryHandler> _logger;

        public GetUserProfileByUserIdQueryHandler(IUserProfileDbContext dbContext, IMapper mapper, ILogger<GetUserProfileByUserIdQueryHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<UserProfileDto> Handle(GetUserProfileByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userProfile = await _dbContext.UserProfiles.FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

            if (userProfile is null)
            {
                _logger.LogWarning("User Profile with for user id: {@Id} was not found.", request.UserId);
                throw new NotFoundException(nameof(Domain.Entities.UserProfile));
            }

            return _mapper.Map<UserProfileDto>(userProfile);
        }
    }
}