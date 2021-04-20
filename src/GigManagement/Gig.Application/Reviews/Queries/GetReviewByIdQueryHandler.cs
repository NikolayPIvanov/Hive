using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Reviews.Queries
{
    public record ReviewDto(string Comment, double Rating);
    
    public record GetReviewByIdQuery(int GigId, int ReviewId) : IRequest<ReviewDto>;
    
    public class GetReviewByIdQueryHandler : IRequestHandler<GetReviewByIdQuery, ReviewDto>
    {
        private readonly IGigManagementDbContext _context;
        private readonly IMapper _mapper;

        public GetReviewByIdQueryHandler(IGigManagementDbContext context, IMapper mapper)
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