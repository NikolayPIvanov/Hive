using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.GigScopes.Command
{
    public record CreateGigScopeCommand(int GigId, string Description) : IRequest<int>;

    public class CreateGigScopeValidator : AbstractValidator<CreateGigScopeCommand>
    {
        public CreateGigScopeValidator(IGigManagementDbContext dbContext)
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
        private readonly IGigManagementDbContext _dbContext;

        public CreateGigScopeCommandHandler(IGigManagementDbContext dbContext)
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