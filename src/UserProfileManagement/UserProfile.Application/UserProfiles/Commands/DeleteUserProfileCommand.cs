﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Exceptions;
using Hive.UserProfile.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hive.UserProfile.Application.UserProfiles.Commands
{
    public record DeleteUserProfileCommand(int UserProfileId) : IRequest;

    public class DeleteUserProfileCommandHandler : IRequestHandler<DeleteUserProfileCommand>
    {
        private readonly IUserProfileDbContext _dbContext;
        private readonly ILogger<DeleteUserProfileCommandHandler> _logger;

        public DeleteUserProfileCommandHandler(IUserProfileDbContext dbContext, ILogger<DeleteUserProfileCommandHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
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

            _dbContext.UserProfiles.Remove(userProfile);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            _logger.LogWarning("User Profile with id: {@Id} was deleted.", request.UserProfileId);

            return Unit.Value;
        }
    }
}