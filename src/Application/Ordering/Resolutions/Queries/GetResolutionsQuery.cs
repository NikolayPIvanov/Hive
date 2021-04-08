using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Ordering.Resolutions.Queries
{
    public record GetResolutionsQuery(Guid OrderNumber) : IRequest<IEnumerable<ResolutionDto>>;

    public class GetResolutionsQueryHandler : IRequestHandler<GetResolutionsQuery, IEnumerable<ResolutionDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetResolutionsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<ResolutionDto>> Handle(GetResolutionsQuery request, CancellationToken cancellationToken)
        {
            var gig = await _context.Orders.Select(x => new { x.Id, x.OrderNumber})
                .FirstOrDefaultAsync(x => x.OrderNumber == request.OrderNumber, cancellationToken: cancellationToken);
            
            return await _context.Resolutions
                .Where(r => r.OrderId == gig.Id)
                .ProjectToListAsync<ResolutionDto>(_mapper.ConfigurationProvider);
        }
    }
}