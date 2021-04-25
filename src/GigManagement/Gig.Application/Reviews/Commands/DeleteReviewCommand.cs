using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.Reviews.Commands
{
    public record DeleteReviewCommand(int Id) : IRequest;
    
    public class DeleteReviewCommandHandler : AuthorizationRequestHandler<Review>, IRequestHandler<DeleteReviewCommand>
    {
        private readonly IGigManagementDbContext _context;
        private readonly ILogger<DeleteReviewCommandHandler> _logger;

        public DeleteReviewCommandHandler(IGigManagementDbContext context, 
            ICurrentUserService currentUserService, IAuthorizationService authorizationService,
            ILogger<DeleteReviewCommandHandler> logger) : base(currentUserService, authorizationService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
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
            
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogWarning("Review with id: {@Id} was deleted", request.Id);
            
            return Unit.Value;
        }
    }
}