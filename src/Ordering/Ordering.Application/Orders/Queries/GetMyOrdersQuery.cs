using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Mappings;
using Hive.Common.Core.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Contracts;
using Ordering.Domain.Entities;

namespace Ordering.Application.Orders.Queries
{
    [Authorize(Roles = "Buyer")]
    public record GetMyOrdersQuery : IRequest<IEnumerable<OrderDto>>;

    public class GetMyOrdersQueryHandler : IRequestHandler<GetMyOrdersQuery, IEnumerable<OrderDto>>
    {
        private readonly IOrderingContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetMyOrdersQueryHandler(IOrderingContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        
        public async Task<IEnumerable<OrderDto>> Handle(GetMyOrdersQuery request, CancellationToken cancellationToken)
        {
            var buyerId = await _context.Buyers.Select(x => new {x.Id, x.UserId})
                .FirstOrDefaultAsync(x => x.UserId == _currentUserService.UserId, cancellationToken);
            if (buyerId == null)
            {
                throw new NotFoundException(nameof(Buyer));
            }
            
            return await _context.Orders
                .Include(o => o.Requirement)
                .Include(o => o.OrderStates)
                .Where(o => o.BuyerId ==  buyerId.Id)
                .AsNoTracking()
                .ProjectToListAsync<OrderDto>(_mapper.ConfigurationProvider);
        }
    }
    
}