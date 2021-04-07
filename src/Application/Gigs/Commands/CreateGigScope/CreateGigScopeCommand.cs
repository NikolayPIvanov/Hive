using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Gigs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Gigs.Commands.CreateGigScope
{
    public record CreateGigScopeCommand(int GigId, string Description) : IRequest<int>;

    public class CreateGigScopeValidator : AbstractValidator<CreateGigScopeCommand>
    {
        public CreateGigScopeValidator(IApplicationDbContext dbContext)
        {
            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Description should be below 2000 characters.")
                .MinimumLength(10).WithMessage("Description should be above 10 characters.")
                .NotNull().WithMessage("Must provide description for gig.");

            RuleFor(x => x.GigId)
                .MustAsync(async (id, token) => await dbContext.Gigs.AnyAsync(x => x.Id == id, token))
                .WithMessage("Must specify an existing gig.");

            RuleFor(x => x.GigId)
                .MustAsync(async (id, token) => !(await dbContext.GigScopes.AnyAsync(x => x.GigId == id, token)))
                .WithMessage("This gig already contains scope.");
        }
    }

    public class CreateGigScopeCommandHandler : IRequestHandler<CreateGigScopeCommand, int>
    {
        private readonly IApplicationDbContext _dbContext;

        public CreateGigScopeCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<int> Handle(CreateGigScopeCommand request, CancellationToken cancellationToken)
        {
            var gigScope = new GigScope(request.Description, request.GigId);

            var gig = await _dbContext.Gigs.FindAsync(request.GigId);
            gig.GigScope = gigScope;

            _dbContext.GigScopes.Add(gigScope);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return gigScope.Id;
        }
    }
}