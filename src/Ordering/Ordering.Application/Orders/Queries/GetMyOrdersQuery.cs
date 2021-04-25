using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Application.Orders.Queries
{
    public record GetMyOrdersQuery : IRequest<IEnumerable<OrderDto>>;

    public class GetMyOrdersQueryHandler : IRequestHandler<GetMyOrdersQuery, IEnumerable<OrderDto>>
    {
        private readonly IOrderingContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<GetMyOrdersQueryHandler> _logger;

        public GetMyOrdersQueryHandler(IOrderingContext context, IMapper mapper, ICurrentUserService currentUserService, 
            ILogger<GetMyOrdersQueryHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<IEnumerable<OrderDto>> Handle(GetMyOrdersQuery request, CancellationToken cancellationToken)
        {
            var buyerId = await _context.Buyers.Select(x => new {x.Id, x.UserId})
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == _currentUserService.UserId, cancellationToken);
            
            if (buyerId == null)
            {
                _logger.LogWarning("Buyer with user id: {@Id} was not found", _currentUserService.UserId);
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