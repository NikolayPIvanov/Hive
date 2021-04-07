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
    public record GetSellerOrdersQuery(string SellerUserId) : IRequest<IEnumerable<OrderDto>>;

    public class GetSellerOrdersQueryHandler : IRequestHandler<GetSellerOrdersQuery, IEnumerable<OrderDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetSellerOrdersQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<OrderDto>> Handle(GetSellerOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders
                .Include(o => o.Requirement)
                .Include(o => o.OrderStates)
                .Where(o => o.SellerId == request.SellerUserId)
                .AsNoTracking()
                .ProjectToListAsync<OrderDto>(_mapper.ConfigurationProvider);

            return orders;
        }
    }
}