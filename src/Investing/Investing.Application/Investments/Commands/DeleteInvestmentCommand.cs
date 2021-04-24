using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Investing.Application.Investments.Commands
{
    public record DeleteInvestmentCommand(int Id) : IRequest;

    public class DeleteInvestmentCommandValidator : AbstractValidator<DeleteInvestmentCommand>
    {
        public DeleteInvestmentCommandValidator(IInvestingDbContext context)
        {
            RuleFor(x => x.Id)
                .MustAsync(async (id, token) => 
                    await context.Investments.AnyAsync(x => x.Id == id && !x.IsAccepted, token))
                .WithMessage("Cannot delete an investment that is already accepted by the vendor.");
        }
    }
    
    public class DeleteInvestmentCommandHandler : IRequestHandler<DeleteInvestmentCommand>
    {
        private readonly IInvestingDbContext _context;
        private readonly ILogger<DeleteInvestmentCommandHandler> _logger;

        public DeleteInvestmentCommandHandler(IInvestingDbContext context, ILogger<DeleteInvestmentCommandHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(DeleteInvestmentCommand request, CancellationToken cancellationToken)
        {
            var investment = await _context.Investments.FindAsync(request.Id);
            if (investment == null)
            {
                _logger.LogWarning("Investment with Id {InvestmentId} was not found.", request.Id);
                throw new NotFoundException(nameof(Investment), request.Id);
            }

            _context.Investments.Remove(investment);
            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Investment with Id {InvestmentId} was deleted", request.Id);
            
            return Unit.Value;
        }
    }
}