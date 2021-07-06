using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hive.Common.Core.Mappings;
using Hive.Common.Core.Models;
using Hive.Gig.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Gigs.Queries
{
    public record GetRandomGigsQuery(int PageNumber = 1, int PageSize = 10, string? SearchKey = null) : IRequest<PaginatedList<GigOverviewDto>>;

    public class GetRandomGigsQueryHandler : IRequestHandler<GetRandomGigsQuery, PaginatedList<GigOverviewDto>>
    {
        private readonly IGigManagementDbContext _context;
        private readonly IMapper _mapper;

        public GetRandomGigsQueryHandler(IGigManagementDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<PaginatedList<GigOverviewDto>> Handle(GetRandomGigsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Gigs
                .AsNoTracking()
                .Include(g => g.Seller)
                .Include(g => g.Packages);

            var intermediateQuery =
                query
                    .OrderByDescending(g => g.Created)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize);

            if (!string.IsNullOrEmpty(request.SearchKey))
            {
                var key = request.SearchKey.ToLowerInvariant();
                intermediateQuery = intermediateQuery.Where(g => g.Title.Contains(key));
            }

            var list = await intermediateQuery.ToListAsync(cancellationToken);
            
            var count = await query.CountAsync(cancellationToken);
            
            var dtos = _mapper.Map<ICollection<GigOverviewDto>>(list);

            return new PaginatedList<GigOverviewDto>(dtos.ToList(), count, request.PageNumber, request.PageSize);
        }
    }
}