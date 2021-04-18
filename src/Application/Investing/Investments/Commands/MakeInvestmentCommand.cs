using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Investing;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Investing.Investments.Commands
{
    public record MakeInvestmentCommand(DateTime EffectiveDate, DateTime? ExpirationDate,
            decimal Amount, double RoiPercentage, int PlanId) : IRequest<int>;

    public class MakeInvestmentCommandValidator : AbstractValidator<MakeInvestmentCommand>
    {
        public MakeInvestmentCommandValidator(IApplicationDbContext context)
        {
            RuleFor(x => x.EffectiveDate)
                .Must(x => DateTime.UtcNow.AddYears(1) > x && x >= DateTime.UtcNow);
            
            RuleFor(x => x.ExpirationDate)
                .GreaterThan(x => x.EffectiveDate).When(x => x.ExpirationDate.HasValue);

            RuleFor(x => x.Amount)
                .InclusiveBetween(100.0m, 10_000.0m);

            RuleFor(x => x.RoiPercentage)
                .InclusiveBetween(1.0, 50.0);

            RuleFor(x => x.PlanId)
                .MustAsync(async (id, token) => await context.Plans.AnyAsync(x => x.Id == id && !x.IsFunded, token));
        }
    }

    public class MakeInvestmentCommandHandler : IRequestHandler<MakeInvestmentCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public MakeInvestmentCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        
        public async Task<int> Handle(MakeInvestmentCommand request, CancellationToken cancellationToken)
        {
            var plan =  await _context.Plans.AnyAsync(x => x.Id == request.PlanId, cancellationToken);
            if (!plan)
            {
                throw new NotFoundException(nameof(Plan), request.PlanId);
            }
            
            var investor = await _context.Investors
                .Select(x => new { x.Id, x.UserId })
                .FirstOrDefaultAsync(x => x.UserId == _currentUserService.UserId, cancellationToken);

            var investment = new Investment(request.EffectiveDate, request.ExpirationDate, request.Amount,
                request.RoiPercentage, investor.Id, request.PlanId);

            _context.Investments.Add(investment);
            await _context.SaveChangesAsync(cancellationToken);

            return investment.Id;
        }
    }
}