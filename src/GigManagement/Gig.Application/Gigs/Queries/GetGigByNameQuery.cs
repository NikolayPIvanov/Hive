using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hive.Common.Core.Mappings;
using Hive.Gig.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Gigs.Queries
{
    public record GetGigByNameQuery(string Value) : IRequest<ICollection<GigOverviewDto>>;

    public class GetGigByNameQueryHandler : IRequestHandler<GetGigByNameQuery, ICollection<GigOverviewDto>>
    {
        private readonly IGigManagementDbContext _context;
        private readonly IMapper _mapper;

        public GetGigByNameQueryHandler(IGigManagementDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper;
        }
        
        public async Task<ICollection<GigOverviewDto>> Handle(GetGigByNameQuery request, CancellationToken cancellationToken)
        {
            var list = await _context.Gigs
                .Include(g => g.Packages)
                .Include(g => g.Seller)
                .Where(g => g.Title.ToLower().Contains(request.Value.ToLower()))
                .Take(5)
                .AsNoTracking()
                .ProjectToListAsync<GigOverviewDto>(_mapper.ConfigurationProvider);

            return list;
        }
    }
}