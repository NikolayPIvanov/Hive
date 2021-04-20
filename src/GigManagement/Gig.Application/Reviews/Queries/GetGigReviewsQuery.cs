using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Mappings;
using Hive.Common.Core.Models;
using Hive.Gig.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Reviews.Queries
{
    public record GetGigReviewsQuery(int GigId, int PageNumber, int PageSize) : IRequest<PaginatedList<ReviewDto>>;

    public class GetGigReviewsQueryHandler : IRequestHandler<GetGigReviewsQuery, PaginatedList<ReviewDto>>
    {
        private readonly IGigManagementDbContext _context;
        private readonly IMapper _mapper;

        public GetGigReviewsQueryHandler(IGigManagementDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<PaginatedList<ReviewDto>> Handle(GetGigReviewsQuery request, CancellationToken cancellationToken)
        {
            var gig = await _context.Gigs.AnyAsync(x => x.Id == request.GigId, cancellationToken);

            if (!gig)
            {
                throw new NotFoundException(nameof(Gig), request.GigId);
            }

            return await _context.Reviews.ProjectTo<ReviewDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}