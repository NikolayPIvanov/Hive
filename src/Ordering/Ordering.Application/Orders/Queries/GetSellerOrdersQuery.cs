using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Mappings;
using Hive.Common.Core.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Contracts;

namespace Ordering.Application.Orders.Queries
{
    [Authorize(Roles = "Seller, Administrator")]
    public record GetSellerOrdersQuery(string SellerId) : IRequest<IEnumerable<OrderDto>>;

    public class GetSellerOrdersQueryHandler : IRequestHandler<GetSellerOrdersQuery, IEnumerable<OrderDto>>
    {
        private readonly IOrderingContext _context;
        private readonly IMapper _mapper;

        public GetSellerOrdersQueryHandler(IOrderingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<OrderDto>> Handle(GetSellerOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _context.Orders
                .Where(o => o.SellerId == request.SellerId)
                .AsNoTracking()
                .ProjectToListAsync<OrderDto>(_mapper.ConfigurationProvider);
        }
    }
    
}