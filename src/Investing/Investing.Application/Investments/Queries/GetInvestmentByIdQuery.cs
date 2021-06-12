using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Investing.Application.Investments.Queries
{
    public class InvestmentDto
    {
        public int Id { get; set; }
        
        public DateTime EffectiveDate { get; set; }
        
        public DateTime? ExpirationDate { get; set; }

        public decimal Amount { get; set; }
        
        public double RoiPercentage { get; set; }

        public int InvestorId { get; set; }

        public int PlanId { get; set; }

        public bool IsAccepted { get; set; }
    }
    
    public record GetInvestmentByIdQuery(int PlanId, int Id) : IRequest<InvestmentDto>;
    
    public class GetInvestmentByIdQueryHandler : IRequestHandler<GetInvestmentByIdQuery, InvestmentDto>
    {
        private readonly IInvestingDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetInvestmentByIdQueryHandler> _logger;

        public GetInvestmentByIdQueryHandler(IInvestingDbContext context, IMapper mapper, ILogger<GetInvestmentByIdQueryHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<InvestmentDto> Handle(GetInvestmentByIdQuery request, CancellationToken cancellationToken)
        {
            var plan = await _context.Plans
                .Include(x => x.Investments.Where(i => i.PlanId == request.PlanId))
                .FirstOrDefaultAsync(p => p.Id == request.PlanId, cancellationToken);

            if (plan == null)
            {
                _logger.LogWarning("Plan with id: {@Id} was not found", request.PlanId);
                throw new NotFoundException(nameof(Plan), request.PlanId);
            }

            var investment = plan.Investments.FirstOrDefault();
            if (investment == null)
            {
                _logger.LogWarning("Investment with id: {@Id} was not found", request.Id);
                throw new NotFoundException(nameof(Investment), request.Id);
            }

            return _mapper.Map<InvestmentDto>(investment);
        }
    }
}