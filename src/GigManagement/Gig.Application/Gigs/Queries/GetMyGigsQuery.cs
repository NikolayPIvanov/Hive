using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Mappings;
using Hive.Common.Core.Models;
using Hive.Gig.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.Gigs.Queries
{
    public record GetMyGigsQuery(int PageSize = 8, int PageNumber = 1, string? SearchKey = null) : IRequest<PaginatedList<GigOverviewDto>>;

    public class GetMyGigsQueryHandler : IRequestHandler<GetMyGigsQuery, PaginatedList<GigOverviewDto>>
    {
        private readonly IGigManagementDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ILogger<GetMyGigsQueryHandler> _logger;

        public GetMyGigsQueryHandler(IGigManagementDbContext context, ICurrentUserService currentUserService, IMapper mapper, ILogger<GetMyGigsQueryHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }
        
        public async Task<PaginatedList<GigOverviewDto>> Handle(GetMyGigsQuery request, CancellationToken cancellationToken)
        {
            var seller = await _context.Sellers
                .Select(x => new { x.Id, x.UserId })
                .FirstOrDefaultAsync(s => s.UserId == _currentUserService.UserId, cancellationToken);

            if (seller == null)
            {
                _logger.LogWarning($"Seller Account does not exist for {_currentUserService.UserId}");
                throw new NotFoundException($"Seller Account does not exist for {_currentUserService.UserId}");
            }

            var skip = (request.PageNumber - 1) * request.PageSize;
            var take = request.PageSize;

            var query = _context.Gigs.AsNoTracking().Where(g => g.SellerId == seller.Id);
            var count = await query.CountAsync(cancellationToken);

            if (!string.IsNullOrEmpty(request.SearchKey))
            {
                query = query.Where(q => q.Title.Contains(request.SearchKey.ToLowerInvariant()));
            }
            
            var gigs = await 
                query.Include(g => g.Seller)
                    .Include(g => g.Packages)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync(cancellationToken);

            var mappedGigs = _mapper.Map<ICollection<GigOverviewDto>>(gigs);
            return new PaginatedList<GigOverviewDto>(mappedGigs.ToList(), count, request.PageNumber, request.PageSize);
        }
    }
}