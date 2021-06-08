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
    public record GetMyGigsQuery(int PageSize = 10, int PageNumber = 1) : IRequest<PaginatedList<GigOverviewDto>>;

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

            var list = await
                _context.Gigs
                    .AsNoTracking()
                    .Include(g => g.Seller)
                    .Include(g => g.Packages)
                    .Where(g => g.SellerId == seller.Id)
                    .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<ICollection<GigOverviewDto>>(list);

            return new PaginatedList<GigOverviewDto>(dtos.ToList(), dtos.Count, request.PageNumber, request.PageSize);
        }
    }
}