using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;

namespace Hive.Gig.Application.Reviews.Commands
{
    public record DeleteReviewCommand(int Id) : IRequest;
    
    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand>
    {
        private readonly IGigManagementDbContext _context;

        public DeleteReviewCommandHandler(IGigManagementDbContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _context.Reviews.FindAsync(request.Id);

            if (review == null)
            {
                throw new NotFoundException(nameof(Review), request.Id);
            }
            
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}