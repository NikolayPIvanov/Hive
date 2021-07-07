using System;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using Hive.Common.Core;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using Investing.Contracts.IntegrationEvents;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Investing.Application.Investments.Commands
{
    public record ProcessInvestmentCommand(int PlanId, int InvestmentId, bool Accept = true) : IRequest;

    public class ProcessInvestmentCommandHandler : IRequestHandler<ProcessInvestmentCommand>
    {
        private readonly IInvestingDbContext _context;
        private readonly IIntegrationEventPublisher _publisher;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<ProcessInvestmentCommandHandler> _logger;

        public ProcessInvestmentCommandHandler(IInvestingDbContext context, IIntegrationEventPublisher publisher,
            ICurrentUserService currentUserService, ILogger<ProcessInvestmentCommandHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(ProcessInvestmentCommand request, CancellationToken cancellationToken)
        {
            var investment = await _context.Investments
                .Include(i => i.Plan)
                .ThenInclude(p => p.Vendor)
                .Include(x => x.Investor)
                .FirstOrDefaultAsync(x => x.Id == request.InvestmentId, cancellationToken: cancellationToken);

            if (investment == null)
            {
                _logger.LogWarning("Investment with Id {@InvestmentId} was not found.", request.InvestmentId);
                throw new NotFoundException(nameof(Investment), request.InvestmentId);
            }
            
            if (investment?.Plan == null)
            {
                _logger.LogWarning("Plan with Id {@PlanId} was not found.", request.PlanId);
                throw new NotFoundException(nameof(Plan), request.PlanId);
            }

            var canProcess = _currentUserService.UserId == investment.Plan.Vendor.UserId;
            
            if (!canProcess)
            {
                throw new ForbiddenAccessException();
            }
            

            if (request.Accept)
            {
                // Goes to Billing to check the balance and gets the amount
                // from the investor's wallet to transfer to seller's
                await _publisher.PublishAsync(
                    new InvestmentAcceptedIntegrationEvent(
                        investment.Investor.UserId, 
                        investment.Plan.Vendor.UserId,
                        investment.Id,
                        investment.Amount), cancellationToken);
            }
            else
            {
                investment.IsAccepted = false;
                await _context.SaveChangesAsync(cancellationToken);
            }
            
            _logger.LogInformation("Investment {Id} was successfully updated", request.InvestmentId);

            return Unit.Value;
        }
    }
}