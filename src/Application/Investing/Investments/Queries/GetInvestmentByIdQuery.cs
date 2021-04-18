using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Security;
using Hive.Domain.Entities.Investing;
using MediatR;

namespace Hive.Application.Investing.Investments.Queries
{
    public class InvestmentDto
    {
        public DateTime EffectiveDate { get; set; }
        
        public DateTime? ExpirationDate { get; set; }

        public decimal Amount { get; set; }
        
        public double RoiPercentage { get; set; }

        public int InvestorId { get; set; }

        public int PlanId { get; set; }

        public bool IsAccepted { get; set; }
    }
    
    public record GetInvestmentByIdQuery(int Id) : IRequest<InvestmentDto>;
    
    public class GetInvestmentByIdQueryHandler : IRequestHandler<GetInvestmentByIdQuery, InvestmentDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetInvestmentByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<InvestmentDto> Handle(GetInvestmentByIdQuery request, CancellationToken cancellationToken)
        {
            var investment = await _context.Investments.FindAsync(request.Id);

            if (investment == null)
            {
                throw new NotFoundException(nameof(Investment), request.Id);
            }

            return _mapper.Map<InvestmentDto>(investment);
        }
    }
}