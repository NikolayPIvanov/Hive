using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hive.Common.Core.Mappings;
using Hive.Common.Core.Models;
using Hive.Investing.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Investing.Application.Investments.Queries
{
    public record GetInvestmentsByPlanQuery(int PlanId, int PageNumber = 1, int PageSize = 10, bool OnlyAccepted = false) : IRequest<PaginatedList<InvestmentDto>>;

    public class GetInvestmentsByPlanQueryHandler : IRequestHandler<GetInvestmentsByPlanQuery, PaginatedList<InvestmentDto>>
    {
        private readonly IInvestingDbContext _context;
        private readonly IMapper _mapper;

        public GetInvestmentsByPlanQueryHandler(IInvestingDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<PaginatedList<InvestmentDto>> Handle(GetInvestmentsByPlanQuery request, CancellationToken cancellationToken)
        {
            var query = 
                _context.Investments
                    .Include(x => x.Investor)
                .AsNoTracking()
                .Where(i => i.PlanId == request.PlanId);

            var debu = await query.ToListAsync();
            
            query = request.OnlyAccepted ? query.Where(x => x.IsAccepted) : query.Where(x => !x.IsAccepted);
            
            return await query.ProjectTo<InvestmentDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}