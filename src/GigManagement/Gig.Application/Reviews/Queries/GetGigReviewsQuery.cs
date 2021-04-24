using System;
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
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.Reviews.Queries
{
    public record GetGigReviewsQuery(int GigId, int PageNumber, int PageSize) : IRequest<PaginatedList<ReviewDto>>;

    public class GetGigReviewsQueryHandler : IRequestHandler<GetGigReviewsQuery, PaginatedList<ReviewDto>>
    {
        private readonly IGigManagementDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetGigReviewsQueryHandler> _logger;

        public GetGigReviewsQueryHandler(IGigManagementDbContext context, IMapper mapper, ILogger<GetGigReviewsQueryHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<PaginatedList<ReviewDto>> Handle(GetGigReviewsQuery request, CancellationToken cancellationToken)
        {
            var gig = await _context.Gigs.AnyAsync(x => x.Id == request.GigId, cancellationToken);

            if (!gig)
            {
                _logger.LogWarning("Gig with id: {Id} was not found", request.GigId);
                throw new NotFoundException(nameof(Gig), request.GigId);
            }

            return await _context.Reviews.ProjectTo<ReviewDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}