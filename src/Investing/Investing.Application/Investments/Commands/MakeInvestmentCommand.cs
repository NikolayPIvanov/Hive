using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Security;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Investing.Application.Investments.Commands
{
    public record MakeInvestmentCommand(DateTime EffectiveDate, DateTime? ExpirationDate,
            decimal Amount, double RoiPercentage, int PlanId) : IRequest<int>;

    public class MakeInvestmentCommandValidator : AbstractValidator<MakeInvestmentCommand>
    {
        public MakeInvestmentCommandValidator(IInvestingDbContext context)
        {
            RuleFor(x => x.EffectiveDate)
                .Must(x => DateTime.UtcNow.AddYears(1) > x && x >= DateTime.UtcNow)
                .WithMessage("{Property} should be between now and an year from now.");

            RuleFor(x => x.ExpirationDate)
                .GreaterThan(x => x.EffectiveDate)
                    .When(x => x.ExpirationDate.HasValue)
                    .WithMessage("{Property} should be after ExpirationDate")
                .Must(x => x < DateTime.UtcNow.AddYears(1))
                    .When(x => x.ExpirationDate.HasValue)
                    .WithMessage("{Property} should be no further than an year ahead from now.");

            RuleFor(x => x.Amount)
                .InclusiveBetween(100.0m, 10_000.0m)
                .WithMessage("{Property} needed for funding should be between between 100.0 and 10,000.0");

            RuleFor(x => x.RoiPercentage)
                .InclusiveBetween(1.0, 50.0)
                .WithMessage("{Property} should be between between 1.0 and 50.0");

            RuleFor(x => x.PlanId)
                .MustAsync(async (id, token) => await context.Plans.AnyAsync(x => x.Id == id && !x.IsFunded, token))
                .WithMessage("Could not find plan id with the given id.");
        }
    }

    public class MakeInvestmentCommandHandler : IRequestHandler<MakeInvestmentCommand, int>
    {
        private readonly IInvestingDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<MakeInvestmentCommandHandler> _logger;

        public MakeInvestmentCommandHandler(IInvestingDbContext context, ICurrentUserService currentUserService, 
            ILogger<MakeInvestmentCommandHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<int> Handle(MakeInvestmentCommand request, CancellationToken cancellationToken)
        {
            var investor = await _context.Investors
                .Select(x => new { x.Id, x.UserId })
                .FirstOrDefaultAsync(x => x.UserId == _currentUserService.UserId, cancellationToken);

            var investment = new Investment(request.EffectiveDate, request.ExpirationDate, request.Amount,
                request.RoiPercentage, investor.Id, request.PlanId);

            _context.Investments.Add(investment);
            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Investment for {PlanId} was created", request.PlanId);

            return investment.Id;
        }
    }
}