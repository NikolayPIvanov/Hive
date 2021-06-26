using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using Hive.Common.Core;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Security.Handlers;
using Hive.UserProfile.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Hive.UserProfile.Application.UserProfiles.Commands
{
    public record DeleteUserProfileCommand(int UserProfileId) : IRequest;

    public class DeleteUserProfileCommandHandler : AuthorizationRequestHandler<Domain.Entities.UserProfile>,  IRequestHandler<DeleteUserProfileCommand>
    {
        private readonly IUserProfileDbContext _dbContext;
        private readonly IFileService _fileService;
        private readonly ILogger<DeleteUserProfileCommandHandler> _logger;

        public DeleteUserProfileCommandHandler(IUserProfileDbContext dbContext,
            ICurrentUserService currentUserService, IAuthorizationService authorizationService,
            IFileService fileService,
            ILogger<DeleteUserProfileCommandHandler> logger) : base(currentUserService, authorizationService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
        {
            var userProfile = await _dbContext.UserProfiles.FindAsync(new[] {request.UserProfileId}, cancellationToken);

            if (userProfile is null)
            {
                _logger.LogWarning("User Profile with id: {@Id} was not found.", request.UserProfileId);
                throw new NotFoundException(nameof(Domain.Entities.UserProfile), request.UserProfileId);
            }
            
            var result = await base.AuthorizeAsync(userProfile,  new [] {"OnlyOwnerPolicy"});
            
            if (!result.All(s => s.Succeeded))
            {
                throw new ForbiddenAccessException();
            }

            if (!string.IsNullOrEmpty(userProfile.AvatarFile))
            {
                var deleted = await _fileService.DeleteAsync("user-avatars", userProfile.AvatarFile, cancellationToken);                
            }

            _dbContext.UserProfiles.Remove(userProfile);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            _logger.LogWarning("User Profile with id: {@Id} was deleted.", request.UserProfileId);

            return Unit.Value;
        }
    }
}