using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Mappings;
using Hive.Domain.Entities.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Ordering.Orders.Queries
{
    public record GetMyOrdersQuery : IRequest<IEnumerable<OrderDto>>;

    public class GetMyOrdersQueryHandler : IRequestHandler<GetMyOrdersQuery, IEnumerable<OrderDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetMyOrdersQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        
        public async Task<IEnumerable<OrderDto>> Handle(GetMyOrdersQuery request, CancellationToken cancellationToken)
        {
            var buyerId = await GetBuyerAsync(cancellationToken);
            return await _context.Orders
                .Where(o => o.BuyerId == buyerId)
                .AsNoTracking()
                .ProjectToListAsync<OrderDto>(_mapper.ConfigurationProvider);
        }
        
        private async Task<int> GetBuyerAsync(CancellationToken cancellationToken)
        {
            var buyer = await _context.Buyers.Select(b => new {b.Id, b.UserId})
                .FirstOrDefaultAsync(b => b.UserId == _currentUserService.UserId, cancellationToken: cancellationToken);

            if (buyer == null)
            {
                throw new NotFoundException(nameof(Buyer));
            }

            return buyer.Id;
        }
    }
}