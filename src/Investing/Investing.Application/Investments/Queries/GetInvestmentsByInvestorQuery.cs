﻿using System;
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
    public record GetInvestmentsByInvestorQuery(int InvestorId) : IRequest<IEnumerable<InvestmentDto>>;

    public class GetInvestmentsByInvestorQueryHandler : IRequestHandler<GetInvestmentsByInvestorQuery, IEnumerable<InvestmentDto>>
    {
        private readonly IInvestingDbContext _context;
        private readonly IMapper _mapper;

        public GetInvestmentsByInvestorQueryHandler(IInvestingDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<IEnumerable<InvestmentDto>> Handle(GetInvestmentsByInvestorQuery request, CancellationToken cancellationToken)
        {
            return await _context.Investments.Include(x => x.Investor)
                .AsNoTracking()
                .Where(i => i.InvestorId == request.InvestorId)
                .ProjectToListAsync<InvestmentDto>(_mapper.ConfigurationProvider);
        }
    }
}