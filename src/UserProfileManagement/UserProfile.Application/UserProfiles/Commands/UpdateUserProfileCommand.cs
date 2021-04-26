#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.UserProfile.Application.Interfaces;
using Hive.UserProfile.Application.UserProfiles.Queries;
using Hive.UserProfile.Domain;
using Hive.UserProfile.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Hive.UserProfile.Application.UserProfiles.Commands
{
    using Domain.Entities;
    
    public record UpdateUserProfileCommand(int UserProfileId, string? FirstName, string? LastName, string? Description, string? Education,
        NotificationSettingDto NotificationSetting, ICollection<string> Skills, ICollection<string> Languages) : IRequest;
    
    public class UpdateUserProfileCommandHandler : AuthorizationRequestHandler<UserProfile>, IRequestHandler<UpdateUserProfileCommand>
    {
        private readonly IUserProfileDbContext _dbContext;
        private readonly ILogger<UpdateUserProfileCommandHandler> _logger;

        public UpdateUserProfileCommandHandler(IUserProfileDbContext dbContext,
            ICurrentUserService currentUserService, IAuthorizationService authorizationService,
            ILogger<UpdateUserProfileCommandHandler> logger) : base(currentUserService, authorizationService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
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

            userProfile.FirstName = request.FirstName;
            userProfile.LastName = request.LastName;
            userProfile.Description = request.Description;
            userProfile.Education = request.Education;
            userProfile.NotificationSetting = new NotificationSetting(request.NotificationSetting.EmailNotifications);
            SetSkills(request, userProfile);
            SetLanguages(request, userProfile);

            await _dbContext.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("User Profile was updated: {@Id} successfully.");

            return Unit.Value;
        }

        private static void SetLanguages(UpdateUserProfileCommand request, Domain.Entities.UserProfile userProfile)
        {
            userProfile.Languages.Clear();
            foreach (var language in request.Languages)
            {
                userProfile.Languages.Add(new Language(language));
            }
        }

        private static void SetSkills(UpdateUserProfileCommand request, Domain.Entities.UserProfile userProfile)
        {
            userProfile.Skills.Clear();
            foreach (var skill in request.Skills)
            {
                userProfile.Skills.Add(new Skill(skill));
            }
        }
    }
}