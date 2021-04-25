using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Application.Resolutions.Queries
{
    public record GetResolutionsQuery(Guid OrderNumber) : IRequest<IEnumerable<ResolutionDto>>;

    public class GetResolutionsQueryHandler : IRequestHandler<GetResolutionsQuery, IEnumerable<ResolutionDto>>
    {
        private readonly IOrderingContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetResolutionsQueryHandler> _logger;

        public GetResolutionsQueryHandler(IOrderingContext context, IMapper mapper, ILogger<GetResolutionsQueryHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<IEnumerable<ResolutionDto>> Handle(GetResolutionsQuery request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.Select(x => new { x.Id, x.OrderNumber})
                .FirstOrDefaultAsync(x => x.OrderNumber == request.OrderNumber, cancellationToken);

            if (order == null)
            {
                _logger.LogWarning("Order with number: {@Id} was not found", request.OrderNumber);
                throw new NotFoundException(nameof(Order), request.OrderNumber);
            }
            
            return await _context.Resolutions
                .Where(r => r.OrderId == order.Id)
                .ProjectToListAsync<ResolutionDto>(_mapper.ConfigurationProvider);
        }
    }
}