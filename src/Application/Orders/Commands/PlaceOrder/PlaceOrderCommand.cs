using System;
using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Orders;
using MediatR;

namespace Hive.Application.Orders.Commands.PlaceOrder
{
    public class PlaceOrderCommand : IRequest<Guid>
    {
        public int OfferedById { get; set; }

        public decimal TotalAmount { get; set; }
        
        public int GigId { get; set; }
        
        public int PackageId { get; set; }

        public string OrderRequirement { get; set; }
    }

    public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public PlaceOrderCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }   
        
        public async Task<Guid> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                GigId = request.GigId,
                PackageId = request.PackageId,
                OfferedById = request.OfferedById,
                TotalAmount = request.TotalAmount,
                Requirement = new Requirement() {Details = request.OrderRequirement},
                OrderedById = await _identityService.GetCurrentUserId()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            return order.OrderNumber;
        }
    }
}