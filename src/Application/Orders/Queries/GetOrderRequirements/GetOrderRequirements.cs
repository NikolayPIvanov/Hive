using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Orders.Queries.GetOrderRequirements
{
    public class GetOrderRequirements : IRequest<RequirementDto>
    {
        public Guid OrderNumber { get; set; }
    }

    public class GetOrderRequirementsHandler : IRequestHandler<GetOrderRequirements, RequirementDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetOrderRequirementsHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<RequirementDto> Handle(GetOrderRequirements request, CancellationToken cancellationToken)
        {
            var entity = await _context.Orders
                .Select(x => new
                {
                    x.OrderNumber,
                    x.Requirement
                })
                .FirstOrDefaultAsync(x => x.OrderNumber == request.OrderNumber, cancellationToken);
            
            if (entity is null)
            {
                throw new NotFoundException(nameof(Order), request.OrderNumber);
            }

            RequirementDto requirement = null;

            if (entity.Requirement != null)
            {
                requirement = _mapper.Map<RequirementDto>(entity.Requirement);
            }

            return requirement;
        }
    }
}