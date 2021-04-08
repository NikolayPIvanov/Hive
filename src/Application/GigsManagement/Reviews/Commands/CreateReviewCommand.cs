using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Gigs;
using MediatR;

namespace Hive.Application.GigsManagement.Reviews.Commands
{
    public class CreateReviewCommand : IRequest<int>
    {
        public int GigId { get; set; }
        
        public double Rating { get; set; }
        
        public string? Comment { get; set; }
    }

    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateReviewCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<int> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var currentUser = "currentUserId";
            var review = new Review(currentUser, request.GigId, request.Comment, request.Rating);

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync(cancellationToken);

            return review.Id;
        }
    }
}