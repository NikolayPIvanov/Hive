﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;

namespace Ordering.Application.Orders.Commands
{
    public record SetInProgressOrderCommand(Guid OrderNumber) : IRequest;
    
    public class SetInProgressOrderCommandHandler : IRequestHandler<SetInProgressOrderCommand>
    {
        private readonly IOrderingContext _context;

        public SetInProgressOrderCommandHandler(IOrderingContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(SetInProgressOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Select(x => new
                {
                    x.OrderNumber,
                    x.OrderStates
                })
                .FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber, cancellationToken: cancellationToken);

            if (order is null)
            {
                throw new NotFoundException(nameof(Order), request.OrderNumber);
            }
            
            var state = new State(OrderState.InProgress, "Order marked in progress");
            order.OrderStates.Add(state);

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}