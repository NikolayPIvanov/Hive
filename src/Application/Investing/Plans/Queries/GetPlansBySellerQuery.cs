using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Mappings;
using Hive.Application.Common.Security;
using MediatR;

namespace Hive.Application.Investing.Plans.Queries
{
    [Authorize(Roles = "Seller, Administrator")]
    public record GetPlansBySellerQuery(int SellerId) : IRequest<IEnumerable<PlanDto>>;

    public class GetPlansBySellerQueryHandler : IRequestHandler<GetPlansBySellerQuery, IEnumerable<PlanDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPlansBySellerQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<PlanDto>> Handle(GetPlansBySellerQuery request, CancellationToken cancellationToken)
        {
            return await _context.Plans
                .Where(p => p.SellerId == request.SellerId)
                .ProjectToListAsync<PlanDto>(_mapper.ConfigurationProvider);
        }
    }
}