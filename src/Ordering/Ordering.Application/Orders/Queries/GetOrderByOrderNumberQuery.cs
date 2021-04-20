using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Contracts;
using Ordering.Domain.Entities;

namespace Ordering.Application.Orders.Queries
{
    public record GetOrderByOrderNumberQuery(Guid OrderNumber) : IRequest<OrderDto>;
    
    public class GetOrderByOrderNumberQueryHandler : IRequestHandler<GetOrderByOrderNumberQuery, OrderDto>
    {
        private readonly IOrderingContext _context;
        private readonly IMapper _mapper;

        public GetOrderByOrderNumberQueryHandler(IOrderingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<OrderDto> Handle(GetOrderByOrderNumberQuery request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
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