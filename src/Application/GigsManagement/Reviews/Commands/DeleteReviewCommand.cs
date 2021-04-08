using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Gigs;
using MediatR;

namespace Hive.Application.GigsManagement.Reviews.Commands
{
    public record DeleteReviewCommand(int Id) : IRequest;
    
    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteReviewCommandHandler(IApplicationDbContext context)
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