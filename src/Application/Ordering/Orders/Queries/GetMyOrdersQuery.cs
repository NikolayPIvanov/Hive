using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Ordering.Orders.Queries
{
    public record GetMyOrdersQuery : IRequest<IEnumerable<OrderDto>>;

    public class GetMyOrdersQueryHandler : IRequestHandler<GetMyOrdersQuery, IEnumerable<OrderDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetMyOrdersQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<OrderDto>> Handle(GetMyOrdersQuery request, CancellationToken cancellationToken)
        {
            // TODO: Use Identity Service
            var userId = "test";
            return await _context.Orders
                .Include(o => o.Requirement)
                .Include(o => o.OrderStates)
                .Where(o => o.BuyerId == 1)
                .AsNoTracking()
                .ProjectToListAsync<OrderDto>(_mapper.ConfigurationProvider);
        }
    }
}