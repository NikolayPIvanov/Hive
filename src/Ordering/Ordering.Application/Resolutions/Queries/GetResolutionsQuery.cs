using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;

namespace Ordering.Application.Resolutions.Queries
{
    public record GetResolutionsQuery(Guid OrderNumber) : IRequest<IEnumerable<ResolutionDto>>;

    public class GetResolutionsQueryHandler : IRequestHandler<GetResolutionsQuery, IEnumerable<ResolutionDto>>
    {
        private readonly IOrderingContext _context;
        private readonly IMapper _mapper;

        public GetResolutionsQueryHandler(IOrderingContext context, IMapper mapper)
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