using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Application.Mappings;
using Hive.Investing.Application.Interfaces;
using MediatR;

namespace Hive.Investing.Application.Plans.Queries
{
    // TODO: describe dto
    public record PlanDto;

    public record GetPlansForVendorQuery(int VendorId) : IRequest<IEnumerable<PlanDto>>;

    public class GetPlansForVendorQueryHandler : IRequestHandler<GetPlansForVendorQuery, IEnumerable<PlanDto>>
    {
        private readonly IInvestingDbContext _context;
        private readonly IMapper _mapper;

        public GetPlansForVendorQueryHandler(IInvestingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<PlanDto>> Handle(GetPlansForVendorQuery request, CancellationToken cancellationToken)
        {
            // validate for authorization
            var currentUserId = "str";
            return await _context.Plans
                .Where(p => p.VendorId == request.VendorId)
                .ProjectToListAsync<PlanDto>(_mapper.ConfigurationProvider);
        }
    }
}