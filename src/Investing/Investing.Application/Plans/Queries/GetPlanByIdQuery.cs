﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Investing.Application.Plans.Queries
{
    public record GetPlanByIdQuery(int Id) : IRequest<PlanDto>;

    public class GetPlanByIdQueryHandler : IRequestHandler<GetPlanByIdQuery, PlanDto>
    {
        private readonly IInvestingDbContext _context;
        private readonly IMapper _mapper;

        public GetPlanByIdQueryHandler(IInvestingDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<PlanDto> Handle(GetPlanByIdQuery request, CancellationToken cancellationToken)
        {
            var plan = await _context.Plans
                .Include(x => x.Investments)
                .ThenInclude(x => x.Investor)
                .Include(x => x.Vendor)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            
            if (plan is null)
            {
                throw new NotFoundException(nameof(Plan), request.Id);
            }

            return _mapper.Map<PlanDto>(plan);
        }
    }
}