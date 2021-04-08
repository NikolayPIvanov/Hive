using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Gigs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.GigsManagement.Gigs.Commands.CreateGig
{
    public record QuestionModel(string Title, string Answer);
    public record CreateGigCommand : IRequest<int>
    {
        public string Title { get; private init; }
        public string Description { get; private init; }
        public int CategoryId { get; private init; }
        public HashSet<string> Tags { get; private init; }
        
        public HashSet<QuestionModel> Questions { get; private init; }

        public CreateGigCommand(string title, string description, int categoryId, HashSet<string> tags, HashSet<QuestionModel> questions)
            => (Title, Description, CategoryId, Tags, Questions) = 
                (title, description, categoryId, tags ?? new HashSet<string>(5), questions ?? new HashSet<QuestionModel>());
    }

    public class CreateGigCommandValidator : AbstractValidator<CreateGigCommand>
    {
        public CreateGigCommandValidator(IApplicationDbContext dbContext)
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
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public CreateGigCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
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
            var gig = new Gig(request.Title, request.Description, request.CategoryId, seller.Id, tags);

            _dbContext.Gigs.Add(gig);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return gig.Id;
        }
    }
}