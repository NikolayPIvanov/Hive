using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Gigs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.GigsManagement.Reviews.Queries
{
    public record GetReviewByIdQuery(int GigId, int ReviewId) : IRequest<ReviewDto>;
    
    public class GetReviewByIdQueryHandler : IRequestHandler<GetReviewByIdQuery, ReviewDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetReviewByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<ReviewDto> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            var review =
                await _context.Reviews.FirstOrDefaultAsync(x => x.GigId == request.GigId && x.Id == request.ReviewId,
                    cancellationToken);

            if (review == null)
            {
                throw new NotFoundException(nameof(Review), request.ReviewId);
            }

            return _mapper.Map<ReviewDto>(review);
        }
    }
}