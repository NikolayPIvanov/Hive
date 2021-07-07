#nullable enable
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        }
    }
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, int>
    {
        private readonly IGigManagementDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<CreateReviewCommandHandler> _logger;

        public CreateReviewCommandHandler(IGigManagementDbContext context, ICurrentUserService currentUserService, ILogger<CreateReviewCommandHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<int> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            if (!(await _context.Gigs.AnyAsync(x => x.Id == request.GigId, cancellationToken)))
            {
                _logger.LogInformation("Gig with id: {@Id} was not found", request.GigId);
                throw new NotFoundException(nameof(Domain.Entities.Gig), request.GigId);
            }
            
            var review = new Review(_currentUserService.UserId, request.GigId, request.Comment, request.Rating);

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Review was created for gig with id: {@Id}", request.GigId);

            return review.Id;
        }
    }
    
}