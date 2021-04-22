using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Security;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Investing.Application.Investments.Commands
{
    [Authorize(Roles = "Seller, Administrator")]
    public record ProcessInvestmentCommand(int InvestmentId, bool Accept = true) : IRequest;

    public class ProcessInvestmentCommandHandler : IRequestHandler<ProcessInvestmentCommand>
    {
        private readonly IInvestingDbContext _context;

        public ProcessInvestmentCommandHandler(IInvestingDbContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(ProcessInvestmentCommand request, CancellationToken cancellationToken)
        {
            var investment = await _context.Investments
                .Include(i => i.Plan)
                .FirstOrDefaultAsync(x => x.Id == request.InvestmentId, cancellationToken: cancellationToken);
            
            if (investment == null)
            {
                throw new NotFoundException(nameof(Investment));
            }

            investment.IsAccepted = request.Accept;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}