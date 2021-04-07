using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Ordering.Orders.Queries
{
    public record GetOrderByOrderNumberQuery(Guid OrderNumber) : IRequest<OrderDto>;
    
    public class GetOrderByOrderNumberQueryHandler : IRequestHandler<GetOrderByOrderNumberQuery, OrderDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetOrderByOrderNumberQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<OrderDto> Handle(GetOrderByOrderNumberQuery request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Include(o => o.Requirement)
                .Include(o => o.OrderStates)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber, cancellationToken: cancellationToken);

            if (order is null)
            {
                throw new NotFoundException(nameof(Order), request.OrderNumber);
            }

            return _mapper.Map<OrderDto>(order);
        }
    }
}