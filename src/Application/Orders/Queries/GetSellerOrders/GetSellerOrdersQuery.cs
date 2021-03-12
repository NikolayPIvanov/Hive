using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Mappings;
using Hive.Domain.Enums;
using MediatR;

namespace Hive.Application.Orders.Queries.GetSellerOrders
{
    public static class GetSellerOrdersQuery
    {
        public record Query(int SellerId, OrderStatus? Status = null) : IRequest<IEnumerable<OrderDto>>;
        
        public class Handler : IRequestHandler<Query, IEnumerable<OrderDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            
            public async Task<IEnumerable<OrderDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.Orders.Where(o => o.OfferedById == request.SellerId);

                if (request.Status.HasValue)
                {
                    query = query.Where(o => o.Status == request.Status.Value);
                }

                var orders = await query.ProjectToListAsync<OrderDto>(_mapper.ConfigurationProvider);

                return orders;
            }
        }
    }
}