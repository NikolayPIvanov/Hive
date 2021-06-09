using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Mappings;
using Hive.Common.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Application.Orders.Queries
{
    public record GetMyOrdersQuery(int PageIndex, int PageSize) : IRequest<PaginatedList<OrderDto>>;

    public class GetMyOrdersQueryHandler : IRequestHandler<GetMyOrdersQuery, PaginatedList<OrderDto>>
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
        
        public async Task<PaginatedList<OrderDto>> Handle(GetMyOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _context.Orders
                .Include(o => o.Requirement)
                .Include(o => o.OrderStates)
                .Include(o => o.Buyer)
                .Where(o => o.SellerUserId ==  _currentUserService.UserId && o.Buyer.UserId == _currentUserService.UserId)
                .AsNoTracking()
                .Select(o => new OrderDto(o.Id, o.OrderNumber, o.Created, o.SellerUserId, 
                    o.Buyer.UserId, o.UnitPrice, o.Quantity, o.TotalPrice, o.IsClosed, 
                    o.Requirement.Details, o.PackageId, 
                    o.OrderStates.Select(os => new StateDto(os.OrderState, os.Reason, os.Created)),
                    o.Resolutions.Select(r => new ResolutionDto(r.Version, r.Location, r.IsApproved))
                    ))
                // .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageIndex, request.PageSize);
        }
    }
    
}