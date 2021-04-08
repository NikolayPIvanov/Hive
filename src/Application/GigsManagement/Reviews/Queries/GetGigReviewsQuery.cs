using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Mappings;
using Hive.Application.Common.Models;
using Hive.Domain.Entities.Gigs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.GigsManagement.Reviews.Queries
{
    public record GetGigReviewsQuery(int GigId, int PageNumber, int PageSize) : IRequest<PaginatedList<ReviewDto>>;

    public class GetGigReviewsQueryHandler : IRequestHandler<GetGigReviewsQuery, PaginatedList<ReviewDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetGigReviewsQueryHandler(IApplicationDbContext context, IMapper mapper)
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