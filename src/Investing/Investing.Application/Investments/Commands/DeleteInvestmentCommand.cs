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
using Hive.Investing.Application.Investments.DomainEvents;
using Hive.Investing.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Investing.Application.Investments.Commands
{
    public record DeleteInvestmentCommand(int PlanId, int InvestmentId) : IRequest;

    public class DeleteInvestmentCommandValidator : AbstractValidator<DeleteInvestmentCommand>
    {
        public DeleteInvestmentCommandValidator(IInvestingDbContext context)
        {
            RuleFor(x => x.InvestmentId)
                .MustAsync(async (id, token) =>
                {
                    var investment = await context.Investments.FirstOrDefaultAsync(x => x.Id == id, token);
                    if (investment != null && investment.ExpirationDate < DateTime.UtcNow)
                    {
                        return true;
                    }

                    return investment != null && !investment.IsAccepted;
                })
                .WithMessage("Cannot delete an ongoing investment.");
        }
    }
    
    public class DeleteInvestmentCommandHandler : AuthorizationRequestHandler<Investment>, IRequestHandler<DeleteInvestmentCommand>
    {
        private readonly IInvestingDbContext _context;
        private readonly ILogger<DeleteInvestmentCommandHandler> _logger;

        public DeleteInvestmentCommandHandler(IInvestingDbContext context, IAuthorizationService authorizationService, 
            ICurrentUserService currentUserService, ILogger<DeleteInvestmentCommandHandler> logger) : base(currentUserService, authorizationService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(DeleteInvestmentCommand request, CancellationToken cancellationToken)
        {
            var planWithInvestment = await _context.Plans
                .Include(x => x.Investments.Where(i => i.Id == request.InvestmentId))
                .FirstOrDefaultAsync(x => x.Id == request.PlanId, cancellationToken);
                
            if (planWithInvestment == null)
            {
                _logger.LogWarning("Plan with Id {@PlanId} was not found.", request.PlanId);
                throw new NotFoundException(nameof(Plan), request.PlanId);
            }
            
            if (!planWithInvestment.Investments.Any())
            {
                _logger.LogWarning("Investment with Id {InvestmentId} was not found.", request.InvestmentId);
                throw new NotFoundException(nameof(Investment), request.InvestmentId);
            }

            var investment = planWithInvestment.Investments.First();
            var result = await base.AuthorizeAsync(investment,  new [] {"OnlyOwnerPolicy"});
            
            if (!result.All(s => s.Succeeded))
            {
                throw new ForbiddenAccessException();
            }
            
            investment.AddDomainEvent(new InvestmentWithdrawnEvent());

            _context.Investments.Remove(investment);
            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Investment with Id {InvestmentId} was deleted", request.InvestmentId);
            
            return Unit.Value;
        }
    }
}