﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;
using OrderStatus = Ordering.Domain.Enums.OrderStatus;

namespace Ordering.Application.Orders.Commands
{
    public record CancelOrderCommand(Guid OrderNumber, string Reason) : IRequest;

    public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
    {
        public CancelOrderCommandValidator()
        {
            RuleFor(c => c.Reason)
                .NotEmpty().WithMessage("A {PropertyName} must be provided");
        }
    }

    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand>
    {
        private readonly IOrderingContext _context;

        public CancelOrderCommandHandler(IOrderingContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Select(x => new
                {
                    x.OrderNumber,
                    x.Status
                })
                .FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber, cancellationToken);

            if (order is null)
            {
                throw new NotFoundException(nameof(Order), request.OrderNumber);
            }
            
            // TODO: V2: Check if order is in progress and compensate the seller a given amount.

            order.Status.Status = OrderStatus.Canceled;
            order.Status.Reason = request.Reason;

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}