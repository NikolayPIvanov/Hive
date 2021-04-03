using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Exceptions;
using Hive.UserProfile.Application.Interfaces;
using Hive.UserProfile.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.UserProfile.Application.UserProfiles.Commands
{
    public record UpdateUserProfileCommand(int UserProfileId, string FirstName, string LastName, string? Description, string Education,
        ICollection<string> Skills, ICollection<string> Languages) : IRequest;
    
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand>
    {
        private readonly IUserProfileDbContext _dbContext;

        public UpdateUserProfileCommandHandler(IUserProfileDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<Unit> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var userProfile = await _dbContext.UserProfiles.FindAsync(new[] {request.UserProfileId}, cancellationToken);

            if (userProfile is null)
            {
                throw new NotFoundException(nameof(Domain.UserProfile), request.UserProfileId);
            }

            userProfile.FirstName = request.FirstName;
            userProfile.LastName = request.LastName;
            userProfile.Description = request.Description;
            userProfile.Education = request.Education;
            SetSkills(request, userProfile);
            SetLanguages(request, userProfile);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private static void SetLanguages(UpdateUserProfileCommand request, Domain.UserProfile userProfile)
        {
            userProfile.Languages.Clear();
            foreach (var language in request.Languages)
            {
                userProfile.Languages.Add(new Language(language));
            }
        }

        private static void SetSkills(UpdateUserProfileCommand request, Domain.UserProfile userProfile)
        {
            userProfile.Skills.Clear();
            foreach (var skill in request.Skills)
            {
                userProfile.Skills.Add(new Skill(skill));
            }
        }
    }
}