﻿using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Exceptions;
using Hive.UserProfile.Application.Interfaces;
using MediatR;

namespace Hive.UserProfile.Application.UserProfiles.Commands
{
    public record DeleteUserProfileCommand(int UserProfileId) : IRequest;

    public class DeleteUserProfileCommandHandler : IRequestHandler<DeleteUserProfileCommand>
    {
        private readonly IUserProfileDbContext _dbContext;

        public DeleteUserProfileCommandHandler(IUserProfileDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<Unit> Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
        {
            var userProfile = await _dbContext.UserProfiles.FindAsync(new[] {request.UserProfileId}, cancellationToken);

            if (userProfile is null)
            {
                throw new NotFoundException(nameof(Domain.UserProfile), request.UserProfileId);
            }

            _dbContext.UserProfiles.Remove(userProfile);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}