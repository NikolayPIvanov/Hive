using System;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Exceptions;
using Hive.UserProfile.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.UserProfile.Application.UserProfiles.Commands
{
    public record UpdateUserNamesCommand(int Id, string? FirstName, string? LastName) : IRequest;

    public class UpdateUserNamesCommandHandler : IRequestHandler<UpdateUserNamesCommand>
    {
        private readonly IUserProfileDbContext _context;

        public UpdateUserNamesCommandHandler(IUserProfileDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        public async Task<Unit> Handle(UpdateUserNamesCommand request, CancellationToken cancellationToken)
        {
            var profile =
                await _context.UserProfiles.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (profile == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.UserProfile), request.Id);
            }

            profile.FirstName = request.FirstName;
            profile.LastName = request.LastName;

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}