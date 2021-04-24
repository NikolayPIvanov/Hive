using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Gig.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Gigs.Commands
{
    using Domain.Entities;
        
    public record QuestionModel(string Title, string Answer);

    public record CreateGigCommand(string Title, string Description, int CategoryId, int? PlanId,
        ICollection<string> Tags, ICollection<QuestionModel> Questions) : IRequest<int>;
    
    public class QuestionValidator : AbstractValidator<QuestionModel>
    {
        public QuestionValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(200).WithMessage("Title length must not be above 50 characters.")
                .MinimumLength(3).WithMessage("Title length must not be below 3 characters.")
                .NotEmpty().WithMessage("Title should be provided.");
            
            RuleFor(x => x.Answer)
                .MaximumLength(1000).WithMessage("Answer length must not be above 250 characters.")
                .MinimumLength(3).WithMessage("Answer length must not be below 3 characters.")
                .NotEmpty().WithMessage("Answer should be provided.");
        }
    }
    
    public class CreateGigCommandValidator : AbstractValidator<CreateGigCommand>
    {
        public CreateGigCommandValidator(IGigManagementDbContext dbContext)
        {
            RuleFor(x => x.Title)
                .MaximumLength(100).WithMessage("Title length must not be above 50 characters.")
                .MinimumLength(3).WithMessage("Title length must not be below 3 characters.")
                .NotEmpty().WithMessage("Title should be provided.");

            RuleFor(x => x.CategoryId)
                .MustAsync(async (id, token) => await dbContext.Categories.AnyAsync(x => x.Id == id, cancellationToken: token))
                .WithMessage("Must provide an existing category id.");
            
            RuleFor(x => x.Description)
                .MaximumLength(2500).WithMessage("Description length must not be above 2500 characters.")
                .MinimumLength(10).WithMessage("Description length must not be below 10 characters.")
                .NotEmpty().WithMessage("Description should be provided.");

            RuleFor(x => x.Tags)
                .Must(tags => tags.Count <= 5).WithMessage("Can provide up to 5 tags.");

            RuleForEach(x => x.Tags)
                .MinimumLength(3).WithMessage("Tag length must not be below 3 characters.")
                .MaximumLength(20).WithMessage("Tag length must not be above 20 characters.");
            
            RuleForEach(x => x.Questions)
                .SetValidator(x => new QuestionValidator()).WithMessage("Question is not in correct format");
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

            if (seller == null)
            {
                throw new NotFoundException();
            }
            

            var tags = (request.Tags ?? new List<string>()).Select(t => new Tag(t)).ToHashSet();
            var questions = (request.Questions ?? new List<QuestionModel>()).Select(q => new Question(q.Title, q.Answer)).ToHashSet();
            var gig = new Gig(request.Title, request.Description, request.CategoryId, seller.Id, tags, questions, request.PlanId);

            _dbContext.Gigs.Add(gig);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return gig.Id;
        }
    }
}