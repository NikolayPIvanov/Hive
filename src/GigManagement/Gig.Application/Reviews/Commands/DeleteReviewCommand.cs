using System;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.Reviews.Commands
{
    public record DeleteReviewCommand(int Id) : IRequest;
    
    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand>
    {
        private readonly IGigManagementDbContext _context;
        private readonly ILogger<DeleteReviewCommandHandler> _logger;

        public DeleteReviewCommandHandler(IGigManagementDbContext context, ILogger<DeleteReviewCommandHandler> logger)
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
            
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogWarning("Review with id: {@Id} was deleted", request.Id);
            
            return Unit.Value;
        }
    }
}