using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using Hive.UserProfile.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.UserProfile.Application.UserProfiles.Commands
{
    public record UpdateUserNamesCommand(int Id, string GivenName, string Surname) : IRequest;

    public class UpdateUserNamesCommandValidator : AbstractValidator<UpdateUserNamesCommand>
    {
        public UpdateUserNamesCommandValidator()
        {
            RuleFor(x => x.GivenName)
                .MaximumLength(3)
                .MaximumLength(50)
                .NotEmpty();
            
            RuleFor(x => x.Surname)
                .MaximumLength(3)
                .MaximumLength(50)
                .NotEmpty();
        }
    }

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

            profile.Surname = request.Surname;
            profile.GivenName = request.GivenName;

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}