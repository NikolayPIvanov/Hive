using System;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Investing.Application.Investments.Commands
{
    public record ProcessInvestmentCommand(int InvestmentId, bool Accept = true) : IRequest;

    public class ProcessInvestmentCommandHandler : IRequestHandler<ProcessInvestmentCommand>
    {
        private readonly IInvestingDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<ProcessInvestmentCommandHandler> _logger;

        public ProcessInvestmentCommandHandler(IInvestingDbContext context,
            ICurrentUserService currentUserService, ILogger<ProcessInvestmentCommandHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(ProcessInvestmentCommand request, CancellationToken cancellationToken)
        {
            var investment = await _context.Investments
                .Include(i => i.Plan)
                .ThenInclude(p => p.Vendor)
                .FirstOrDefaultAsync(x => x.Id == request.InvestmentId, cancellationToken: cancellationToken);
            
            if (investment == null)
            {
                _logger.LogWarning("Investment with Id {InvestmentId} was not found.", request.InvestmentId);
                throw new NotFoundException(nameof(Investment), request.InvestmentId);
            }

            var canProcess = _currentUserService.UserId == investment.Plan.Vendor.UserId;
            
            if (!canProcess)
            {
                throw new ForbiddenAccessException();
            }

            investment.IsAccepted = request.Accept;
            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Investment {Id} was successfully updated", request.InvestmentId);

            return Unit.Value;
        }
    }
}