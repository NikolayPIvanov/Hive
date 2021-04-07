using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Gigs;
using MediatR;

namespace Hive.Application.Reviews.Commands
{
    public record UpdateReviewCommand(int Id, double Rating, string? Comment) : IRequest;
    
    public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateReviewCommandHandler(IApplicationDbContext context)
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