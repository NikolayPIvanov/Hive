using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Mappings;
using Hive.Domain.Entities.Orders;
using MediatR;

namespace Hive.Application.Orders.Commands.PlaceOrder
{
    public class PlaceOrderCommand : IRequest<Guid>, IMapFrom<Order>
    {
        public int OfferedById { get; set; }
        
        public int GigId { get; set; }
        
        public int PackageId { get; set; }

        //public decimal PackagePrice { get; set; }

        public decimal TotalAmount { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PlaceOrderCommand, Order>(MemberList.Source);
        }
    }

    public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public PlaceOrderCommandHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _identityService = identityService;
            _mapper = mapper;
        }   
        
        public async Task<Guid> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            
            
            var order = _mapper.Map<Order>(request);

            order.OrderedById = await _identityService.GetCurrentUserId();

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            return order.OrderNumber;
        }
    }
}