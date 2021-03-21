using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Gigs.Commands
{
    public record CreateGigCommand : IRequest<int>
    {
        public string Title { get; init; }
        public int CategoryId { get;  init; }
        public HashSet<string> Tags { get;  init; }

        public CreateGigCommand(string title, int categoryId, HashSet<string> tags)
            => (Title, CategoryId, Tags) = (title, categoryId, tags ?? new HashSet<string>(5));
    }

    public class CreateGigCommandValidator : AbstractValidator<CreateGigCommand>
    {
        public CreateGigCommandValidator(IGigManagementContext context)
        {
            RuleFor(x => x.Title)
                .MaximumLength(50).WithMessage("Title length must not be above 50 characters.")
                .MinimumLength(3).WithMessage("Title length must not be below 3 characters.")
                .NotEmpty().WithMessage("Title should be provided.");

            RuleFor(x => x.CategoryId)
                .MustAsync(async (id, token) => await context.Categories.AnyAsync(x => x.Id == id, cancellationToken: token))
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
        private readonly IGigManagementContext _context;

        public CreateGigCommandHandler(IGigManagementContext context)
        {
            _context = context;
        }
        
        public async Task<int> Handle(CreateGigCommand request, CancellationToken cancellationToken)
        {
            var tags = request.Tags.Select(t => new Tag(t)).ToHashSet();
            var gig = new Domain.Entities.Gig(request.Title, request.CategoryId, tags);

            _context.Gigs.Add(gig);
            await _context.SaveChangesAsync(cancellationToken);

            return gig.Id;
        }
    }
}