using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Exceptions;
using Hive.UserProfile.Application.Interfaces;
using MediatR;

namespace Hive.UserProfile.Application.UserProfiles.Commands
{
    public record DeleteUserProfileCommand(int UserProfileId) : IRequest;

    public class DeleteUserProfileCommandHandler : IRequestHandler<DeleteUserProfileCommand>
    {
        private readonly IUserProfileContext _context;

        public DeleteUserProfileCommandHandler(IUserProfileContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
        {
            var userProfile = await _context.UserProfiles.FindAsync(new[] {request.UserProfileId}, cancellationToken);

            if (userProfile is null)
            {
                throw new NotFoundException(nameof(Domain.UserProfile), request.UserProfileId);
            }

            _context.UserProfiles.Remove(userProfile);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}