using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Accounts;
using MediatR;

namespace Hive.Application.UserProfiles.Commands.CreateUserProfile
{
    public record CreateUserProfileCommand : IRequest<int>
    {
        public string UserId { get; set; }
    }

    public class CreateUserProfileCommandHandler : IRequestHandler<CreateUserProfileCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateUserProfileCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<int> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = new UserProfile(request.UserId);

            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync(cancellationToken);

            return profile.Id;
        }
    }
}