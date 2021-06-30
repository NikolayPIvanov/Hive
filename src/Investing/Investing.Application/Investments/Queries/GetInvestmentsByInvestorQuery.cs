using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Mappings;
using Hive.Investing.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Investing.Application.Investments.Queries
{
    public record GetInvestmentsByInvestorQuery : IRequest<IEnumerable<InvestmentDto>>;

    public class GetInvestmentsByInvestorQueryHandler : IRequestHandler<GetInvestmentsByInvestorQuery, IEnumerable<InvestmentDto>>
    {
        private readonly IInvestingDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetInvestmentsByInvestorQueryHandler(IInvestingDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUserService = currentUserService;
        }
        
        public async Task<IEnumerable<InvestmentDto>> Handle(GetInvestmentsByInvestorQuery request, CancellationToken cancellationToken)
        {
            return await _context.Investments
                .Include(x => x.Investor)
                .AsNoTracking()
                .Where(i => i.Investor.UserId == _currentUserService.UserId)
                .ProjectToListAsync<InvestmentDto>(_mapper.ConfigurationProvider);
        }
    }
}