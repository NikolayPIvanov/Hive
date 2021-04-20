using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Interfaces;
using MediatR;

namespace Hive.Gig.Application.Gigs.Commands
{
    public record UpdateGigCommand(int Id, string Title, string Description, bool IsDraft, int? PlanId,
        ICollection<string> Tags, ICollection<QuestionModel> Questions) : IRequest;

    public class UpdateGigCommandValidator : AbstractValidator<UpdateGigCommand>
    {
        public UpdateGigCommandValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(50).WithMessage("Title length must not be above 50 characters.")
                .MinimumLength(3).WithMessage("Title length must not be below 3 characters.")
                .NotEmpty().WithMessage("Title should be provided.");
            
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
                .SetValidator(x => new QuestionValidator());
        }
    }

    public class UpdateGigCommandHandler : IRequestHandler<UpdateGigCommand>
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateGigCommandHandler(IGigManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        public async Task<Unit> Handle(UpdateGigCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Gigs.FindAsync(request.Id);
            
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