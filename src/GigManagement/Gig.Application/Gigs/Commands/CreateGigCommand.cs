using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Core.Interfaces;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Gigs.Commands
{
    using Domain.Entities;
    
    public record CreateGigCommand : IRequest<int>
    {
        public string Title { get; private init; }
        public int CategoryId { get; private init; }
        public HashSet<string> Tags { get; private init; }

        public CreateGigCommand(string title, int categoryId, HashSet<string> tags)
            => (Title, CategoryId, Tags) = (title, categoryId, tags ?? new HashSet<string>(5));
    }

    public class CreateGigCommandValidator : AbstractValidator<CreateGigCommand>
    {
        public CreateGigCommandValidator(IGigManagementDbContext dbContext)
        {
            RuleFor(x => x.Title)
                .MaximumLength(50).WithMessage("Title length must not be above 50 characters.")
                .MinimumLength(3).WithMessage("Title length must not be below 3 characters.")
                .NotEmpty().WithMessage("Title should be provided.");

            RuleFor(x => x.CategoryId)
                .MustAsync(async (id, token) => await dbContext.Categories.AnyAsync(x => x.Id == id, cancellationToken: token))
                .WithMessage("Must provide an existing category id.");

            RuleFor(x => x.Tags)
                .Must(tags => tags.Count <= 5).WithMessage("Can provide up to 5 tags.");

            RuleForEach(x => x.Tags)
                .MinimumLength(3).WithMessage("Tag length must not be below 3 characters.")
                .MaximumLength(20).WithMessage("Tag length must not be above 20 characters.");
        }    
    }
    
    public class CreateGigCommandHandler : IRequestHandler<CreateGigCommand, int>
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public CreateGigCommandHandler(IGigManagementDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }
        
        public async Task<int> Handle(CreateGigCommand request, CancellationToken cancellationToken)
        {
            var seller =
                await _dbContext.Sellers.FirstOrDefaultAsync(s => s.UserId == _currentUserService.UserId,
                    cancellationToken);
            var tags = request.Tags.Select(t => new Tag(t)).ToHashSet();
            var gig = new Gig(request.Title, request.CategoryId, seller.Id, tags);

            _dbContext.Gigs.Add(gig);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return gig.Id;
        }
    }
}