using System;
using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Gigs;
using MediatR;

namespace Hive.Application.Gigs.Commands.CreateReview
{
    public class CreateReviewCommand : IRequest<int>
    {
        public int GigId { get; set; }

        public string UserId { get; set; }

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
            var review = new Review
            {
                Comment = request.Comment,
                Rating = request.Rating,
                GigId = request.GigId,
                UserId = request.UserId
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync(cancellationToken);

            return review.Id;
        }
    }
}