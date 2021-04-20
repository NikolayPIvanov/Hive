using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Core.Interfaces;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Reviews.Commands
{
    public record CreateReviewCommand(double Rating, string? Comment, int GigId) : IRequest<int>;
    
    public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator(IGigManagementDbContext context)
        {
            RuleFor(x => x.Comment)
                .MaximumLength(50).WithMessage("{PropertyName} cannot have more than {MaxLength} characters.").When(x => !string.IsNullOrEmpty(x.Comment))
                .MinimumLength(3).WithMessage("{PropertyName} cannot have less than {MinLength} characters.").When(x => !string.IsNullOrEmpty(x.Comment))
                .NotEmpty().WithMessage("A {PropertyName} must be provided").When(x => !string.IsNullOrEmpty(x.Comment));

            RuleFor(x => x.Rating)
                .InclusiveBetween(1.0f, 5.0f).WithMessage("Rating must be between {From} to {To}");

            RuleFor(x => x.GigId)
                .MustAsync(async (gigId, token) => await context.Gigs.AnyAsync(x => x.Id == gigId, token));
        }
    }
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, int>
    {
        private readonly IGigManagementDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreateReviewCommandHandler(IGigManagementDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        
        public async Task<int> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = new Review(_currentUserService.UserId, request.GigId, request.Comment, request.Rating);

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync(cancellationToken);

            return review.Id;
        }
    }
    
}