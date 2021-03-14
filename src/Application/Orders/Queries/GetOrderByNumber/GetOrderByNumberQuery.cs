using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Application.Orders.Queries.GetSellerOrders;
using Hive.Domain.Entities.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Orders.Queries.GetOrderByNumber
{
    public class GetOrderByNumberQuery : IRequest<OrderDto>
    {
        public Guid OrderNumber { get; set; }
    }

    public class GetOrderByNumberQueryHandler : IRequestHandler<GetOrderByNumberQuery, OrderDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetOrderByNumberQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<OrderDto> Handle(GetOrderByNumberQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Orders
                .Include(x => x.Requirement)
                .FirstOrDefaultAsync(x => x.OrderNumber == request.OrderNumber, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Order), request.OrderNumber);
            }
            
            return _mapper.Map<OrderDto>(entity);
        }
    }
}