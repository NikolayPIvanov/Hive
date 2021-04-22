using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Security;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using MediatR;

namespace Hive.Investing.Application.Investments.Commands
{
    [Authorize(Roles = "Investor, Administrator")]
    public record UpdateInvestmentCommand(int InvestmentId, DateTime EffectiveDate, DateTime? ExpirationDate,
        decimal Amount, double RoiPercentage) : IRequest;

    public class UpdateInvestmentCommandValidator : AbstractValidator<UpdateInvestmentCommand>
    {
        public UpdateInvestmentCommandValidator(IInvestingDbContext context)
        {
            RuleFor(x => x.EffectiveDate)
                .Must(x => DateTime.UtcNow.AddYears(1) > x && x >= DateTime.UtcNow);
            
            RuleFor(x => x.ExpirationDate)
                .GreaterThan(x => x.EffectiveDate).When(x => x.ExpirationDate.HasValue);

            RuleFor(x => x.Amount)
                .InclusiveBetween(100.0m, 10_000.0m);

            RuleFor(x => x.RoiPercentage)
                .InclusiveBetween(1.0, 50.0);
        }
    }
    
    public class UpdateInvestmentCommandHandler : IRequestHandler<UpdateInvestmentCommand>
    {
        private readonly IInvestingDbContext _context;

        public UpdateInvestmentCommandHandler(IInvestingDbContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(UpdateInvestmentCommand request, CancellationToken cancellationToken)
        {
            var investment = await _context.Investments.FindAsync(request.InvestmentId);
            if (investment == null)
            {
                throw new NotFoundException(nameof(Investment), request.InvestmentId);
            }

            investment.Amount = request.Amount;
            investment.RoiPercentage = request.RoiPercentage;
            investment.EffectiveDate = request.EffectiveDate;
            investment.ExpirationDate = request.ExpirationDate;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}