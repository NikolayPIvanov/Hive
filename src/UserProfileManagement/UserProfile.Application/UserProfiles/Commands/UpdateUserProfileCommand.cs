using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Application.Exceptions;
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
        private readonly IUserProfileContext _context;

        public UpdateUserProfileCommandHandler(IUserProfileContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var userProfile = await _context.UserProfiles.FindAsync(new[] {request.UserProfileId}, cancellationToken);

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

            await _context.SaveChangesAsync(cancellationToken);

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