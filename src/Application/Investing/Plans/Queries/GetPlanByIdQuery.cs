using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Investing;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Investing.Plans.Queries
{
    public class PlanDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int EstimatedReleaseDays { get; set; }
        
        public DateTime? EstimatedReleaseDate { get; set; }

        public decimal FundingNeeded { get; set; }
        
        public int SellerId { get; set; }

        public ICollection<string> Tags { get; set; }
    }
    
    public record GetPlanByIdQuery(int Id) : IRequest<PlanDto>;

    public class GetPlanByIdQueryHandler : IRequestHandler<GetPlanByIdQuery, PlanDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPlanByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<PlanDto> Handle(GetPlanByIdQuery request, CancellationToken cancellationToken)
        {
            var plan = await _context.Plans
                .Include(x => x.Investments)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            
            if (plan is null)
            {
                throw new NotFoundException(nameof(Plan), request.Id);
            }

            return _mapper.Map<PlanDto>(plan);
        }
    }
}