using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.UserProfile.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.UserProfile.Application.UserProfiles.Queries
{
    public record GetUserProfileByIdQuery : IRequest<UserProfileDto>;

    public class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileByIdQuery, UserProfileDto>
    {
        private readonly IUserProfileDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<GetUserProfileByIdQueryHandler> _logger;

        public GetUserProfileByIdQueryHandler(IUserProfileDbContext dbContext, IMapper mapper, 
            ICurrentUserService currentUserService, ILogger<GetUserProfileByIdQueryHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<UserProfileDto> Handle(GetUserProfileByIdQuery request, CancellationToken cancellationToken)
        {
            var userProfile = await _dbContext.UserProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == _currentUserService.UserId, cancellationToken);
            
            if (userProfile is null)
            {
                _logger.LogWarning("User Profile with id: {@Id} was not found.", _currentUserService.UserId);
                throw new NotFoundException(nameof(Domain.Entities.UserProfile), _currentUserService.UserId);
            }

            return _mapper.Map<UserProfileDto>(userProfile);
        }
    }
}