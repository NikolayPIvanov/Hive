using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;

namespace Hive.Gig.Application.Reviews.Commands
{
    public record UpdateReviewCommand(int Id, double Rating, string? Comment) : IRequest;
    
    public class UpdateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
    {
        public UpdateReviewCommandValidator()
        {
            RuleFor(x => x.Comment)
                .MaximumLength(50).WithMessage("{PropertyName} cannot have more than {MaxLength} characters.").When(x => !string.IsNullOrEmpty(x.Comment))
                .MinimumLength(3).WithMessage("{PropertyName} cannot have less than {MinLength} characters.").When(x => !string.IsNullOrEmpty(x.Comment))
                .NotEmpty().WithMessage("A {PropertyName} must be provided").When(x => !string.IsNullOrEmpty(x.Comment));

            RuleFor(x => x.Rating)
                .InclusiveBetween(1.0f, 5.0f).WithMessage("Rating must be between {From} to {To}");
        }
    }
    
    public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand>
    {
        private readonly IGigManagementDbContext _context;

        public UpdateReviewCommandHandler(IGigManagementDbContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _context.Reviews.FindAsync(request.Id);

            if (review == null)
            {
                throw new NotFoundException(nameof(Review), request.Id);
            }

            review.Comment = request.Comment;
            review.Rating = request.Rating;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}