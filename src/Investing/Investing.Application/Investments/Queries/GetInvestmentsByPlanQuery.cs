using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Mappings;
using Hive.Investing.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Investing.Application.Investments.Queries
{
    public record GetInvestmentsByPlanQuery(int PlanId) : IRequest<IEnumerable<InvestmentDto>>;

    public class GetInvestmentsByPlanQueryHandler : IRequestHandler<GetInvestmentsByPlanQuery, IEnumerable<InvestmentDto>>
    {
        private readonly IInvestingDbContext _context;
        private readonly IMapper _mapper;

        public GetInvestmentsByPlanQueryHandler(IInvestingDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<IEnumerable<InvestmentDto>> Handle(GetInvestmentsByPlanQuery request, CancellationToken cancellationToken)
        {
            return await _context.Investments
                .AsNoTracking()
                .Where(i => i.PlanId == request.PlanId)
                .ProjectToListAsync<InvestmentDto>(_mapper.ConfigurationProvider);
        }
    }
}