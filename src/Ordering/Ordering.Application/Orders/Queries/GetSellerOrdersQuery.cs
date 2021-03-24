﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Application.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Contracts;

namespace Ordering.Application.Orders.Queries
{
    public record GetSellerOrdersQuery(int SellerId) : IRequest<IEnumerable<OrderDto>>;

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
            var orders = await _context.Orders
                .Include(o => o.Requirement)
                .Include(o => o.Status)
                .Where(o => o.SellerId == request.SellerId)
                .AsNoTracking()
                .ProjectToListAsync<OrderDto>(_mapper.ConfigurationProvider);

            return orders;
        }
    }
    
}