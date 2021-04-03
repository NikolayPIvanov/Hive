using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.GigScopes.Command
{
    public record UpdateGigScopeCommand(int GigId, string Description) : IRequest;

    public class UpdateGigValidator : AbstractValidator<UpdateGigScopeCommand>
    {
        public UpdateGigValidator()
        {
            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Description should be below 2000 characters.")
                .MinimumLength(10).WithMessage("Description should be above 10 characters.")
                .NotNull().WithMessage("Must provide description for gig.");
        }
    }
    
    public class UpdateGigCommandHandler : IRequestHandler<UpdateGigScopeCommand>
    {
        private readonly IGigManagementDbContext _dbContext;

        public UpdateGigCommandHandler(IGigManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<Unit> Handle(UpdateGigScopeCommand request, CancellationToken cancellationToken)
        {
            var gig = await _dbContext.Gigs
                .Include(g => g.GigScope)
                .FirstOrDefaultAsync(g => g.Id == request.GigId, cancellationToken: cancellationToken);

            if (gig is null)
            {
                throw new NotFoundException(nameof(Gig), request.GigId);
            }

            gig.GigScope.Description = request.Description;
            
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}