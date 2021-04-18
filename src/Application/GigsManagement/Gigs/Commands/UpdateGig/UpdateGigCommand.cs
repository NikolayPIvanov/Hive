using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Security;
using Hive.Application.GigsManagement.Gigs.Commands.CreateGig;
using Hive.Application.Investing.Plans.Queries;
using Hive.Domain.Entities.Gigs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.GigsManagement.Gigs.Commands.UpdateGig
{
    // TODO: Same seller Id
    [Authorize(Roles = "Seller, Administrator")]
    public record UpdateGigCommand : IRequest
    {
        public int Id { get; private set; }
        
        public string Title { get; private set; }

        public string Description { get; private set; }
        
        public bool IsDraft { get; set; } = true;
        
        public int CategoryId { get; private set; }
        
        public int? PlanId { get; private set; }
        
        public HashSet<string> Tags { get; private init; }
        
        public HashSet<QuestionModel> Questions { get; private init; }

        
        public UpdateGigCommand(int id, string title, string description, int categoryId, int? planId, bool isDraft, HashSet<string> tags, HashSet<QuestionModel> questions)
            => (Id, Title, Description, CategoryId, PlanId, IsDraft, Tags, Questions) = 
                (id, title, description, categoryId, planId, isDraft, tags ?? new HashSet<string>(5), questions ?? new HashSet<QuestionModel>());
    }

    public class UpdateGigCommandValidator : AbstractValidator<UpdateGigCommand>
    {
        public UpdateGigCommandValidator(IApplicationDbContext dbContext)
        {
            RuleFor(x => x.Title)
                .MaximumLength(50).WithMessage("Title length must not be above 50 characters.")
                .MinimumLength(3).WithMessage("Title length must not be below 3 characters.")
                .NotEmpty().WithMessage("Title should be provided.");
            
            RuleFor(x => x.Description)
                .MaximumLength(2500).WithMessage("Description length must not be above 2500 characters.")
                .MinimumLength(10).WithMessage("Description length must not be below 10 characters.")
                .NotEmpty().WithMessage("Description should be provided.");

            RuleFor(x => x.CategoryId)
                .MustAsync(async (id, token) => await dbContext.Categories.AnyAsync(x => x.Id == id, cancellationToken: token))
                .WithMessage("Must provide an existing category id.");

            RuleFor(x => x.Tags)
                .Must(tags => tags.Count <= 5).WithMessage("Can provide up to 5 tags.");

            RuleForEach(x => x.Tags)
                .MinimumLength(3).WithMessage("Tag length must not be below 3 characters.")
                .MaximumLength(20).WithMessage("Tag length must not be above 20 characters.");
            
            RuleForEach(x => x.Questions)
                .SetValidator(x => new QuestionValidator());
        }
    }

    public class UpdateGigCommandHandler : IRequestHandler<UpdateGigCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateGigCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        public async Task<Unit> Handle(UpdateGigCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Gigs
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            
            if (entity is null)
            {
                throw new NotFoundException(nameof(Gig), request.Id);
            }

            _mapper.Map(request, entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}