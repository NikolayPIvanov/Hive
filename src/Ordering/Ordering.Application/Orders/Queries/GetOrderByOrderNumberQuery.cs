using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Application.Orders.Queries
{
    public record GetOrderByOrderNumberQuery(Guid OrderNumber) : IRequest<OrderDto>;
    
    public class GetOrderByOrderNumberQueryHandler : IRequestHandler<GetOrderByOrderNumberQuery, OrderDto>
    {
        private readonly IOrderingContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetOrderByOrderNumberQueryHandler> _logger;

        public GetOrderByOrderNumberQueryHandler(IOrderingContext context, IMapper mapper, ILogger<GetOrderByOrderNumberQueryHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<OrderDto> Handle(GetOrderByOrderNumberQuery request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber, cancellationToken);

            if (order is null)
            {
                _logger.LogWarning("Order with order number: {@Id} was not found", request.OrderNumber);
                throw new NotFoundException(nameof(Order), request.OrderNumber);
            }

            return _mapper.Map<OrderDto>(order);
        }
    }
}