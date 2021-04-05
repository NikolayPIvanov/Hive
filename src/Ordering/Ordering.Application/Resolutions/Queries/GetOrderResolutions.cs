using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hive.Common.Core.Mappings;
using Hive.Common.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;

namespace Ordering.Application.Resolutions.Queries
{
    public record ResolutionDto;

    public record GetOrderResolutionsQuery(Guid OrderNumber, int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedList<ResolutionDto>>;

    public class GetOrderResolutionsQueryHandler : IRequestHandler<GetOrderResolutionsQuery, PaginatedList<ResolutionDto>>
    {
        private readonly IOrderingContext _context;
        private readonly IMapper _mapper;

        public GetOrderResolutionsQueryHandler(IOrderingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<PaginatedList<ResolutionDto>> Handle(GetOrderResolutionsQuery request, CancellationToken cancellationToken)
        {
            var orderId = await _context.Orders
                .Select(o => new {o.Id, o.OrderNumber})
                .FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber, cancellationToken);

            return await _context.Resolutions
                .Where(x => x.OrderId == orderId.Id)
                .ProjectTo<ResolutionDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}