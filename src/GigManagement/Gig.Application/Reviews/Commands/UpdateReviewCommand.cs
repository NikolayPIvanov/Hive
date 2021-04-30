using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Core;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Security.Handlers;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

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
    
    public class UpdateReviewCommandHandler : AuthorizationRequestHandler<Review>, IRequestHandler<UpdateReviewCommand>
    {
        private readonly IGigManagementDbContext _context;
        private readonly ILogger<UpdateReviewCommandHandler> _logger;

        public UpdateReviewCommandHandler(IGigManagementDbContext context, 
            ICurrentUserService currentUserService, IAuthorizationService authorizationService,
            ILogger<UpdateReviewCommandHandler> logger) : base(currentUserService, authorizationService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _context.Reviews.FindAsync(request.Id);

            if (review == null)
            {                
                _logger.LogWarning("Review with id: {@Id} was not found", request.Id);
                throw new NotFoundException(nameof(Review), request.Id);
            }
            
            var result = await base.AuthorizeAsync(review,  new [] {"OnlyOwnerPolicy"});
            
            if (!result.All(s => s.Succeeded))
            {
                throw new ForbiddenAccessException();
            }

            review.Comment = request.Comment;
            review.Rating = request.Rating;

            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogWarning("Review with id: {@Id} was updated", request.Id);

            return Unit.Value;
        }
    }
}