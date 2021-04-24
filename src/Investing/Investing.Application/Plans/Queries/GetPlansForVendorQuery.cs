using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Mappings;
using Hive.Common.Core.Security;
using Hive.Investing.Application.Interfaces;
using MediatR;

namespace Hive.Investing.Application.Plans.Queries
{
    public record GetPlansForVendorQuery(int VendorId) : IRequest<IEnumerable<PlanDto>>;

    public class GetPlansForVendorQueryHandler : IRequestHandler<GetPlansForVendorQuery, IEnumerable<PlanDto>>
    {
        private readonly IInvestingDbContext _context;
        private readonly IMapper _mapper;

        public GetPlansForVendorQueryHandler(IInvestingDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<IEnumerable<PlanDto>> Handle(GetPlansForVendorQuery request, CancellationToken cancellationToken)
        {
            return await _context.Plans
                .Where(p => p.VendorId == request.VendorId)
                .ProjectToListAsync<PlanDto>(_mapper.ConfigurationProvider);
        }
    }
}