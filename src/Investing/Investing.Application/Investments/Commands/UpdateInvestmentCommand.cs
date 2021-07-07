using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Core;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Security.Handlers;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Hive.Investing.Application.Investments.Commands
{
    public record UpdateInvestmentCommand(int InvestmentId, DateTime EffectiveDate, DateTime? ExpirationDate,
        decimal Amount, double RoiPercentage) : IRequest;

    public class UpdateInvestmentCommandValidator : AbstractValidator<UpdateInvestmentCommand>
    {
        public UpdateInvestmentCommandValidator(IInvestingDbContext context)
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
        }
    }
    
    public class UpdateInvestmentCommandHandler : AuthorizationRequestHandler<Investment>, IRequestHandler<UpdateInvestmentCommand>
    {
        private readonly IInvestingDbContext _context;
        private readonly ILogger<UpdateInvestmentCommandHandler> _logger;

        public UpdateInvestmentCommandHandler(IInvestingDbContext context, IAuthorizationService authorizationService, 
            ICurrentUserService currentUserService, ILogger<UpdateInvestmentCommandHandler> logger) : base(currentUserService, authorizationService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(UpdateInvestmentCommand request, CancellationToken cancellationToken)
        {
            var investment = await _context.Investments.FindAsync(request.InvestmentId);
            if (investment == null)
            {
                _logger.LogWarning("Investment with Id {InvestmentId} was not found.", request.InvestmentId);
                throw new NotFoundException(nameof(Investment), request.InvestmentId);
            }
            
            var result = await base.AuthorizeAsync(investment,  new [] {"OnlyOwnerPolicy"});
            
            if (!result.All(s => s.Succeeded))
            {
                throw new ForbiddenAccessException();
            }

            investment.Amount = request.Amount;
            investment.RoiPercentage = request.RoiPercentage;
            investment.EffectiveDate = request.EffectiveDate;
            investment.ExpirationDate = request.ExpirationDate;

            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Investment {Id} was successfully updated", request.InvestmentId);

            return Unit.Value;
        }
    }
}