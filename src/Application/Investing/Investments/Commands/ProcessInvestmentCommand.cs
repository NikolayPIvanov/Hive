using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Investing;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Investing.Investments.Commands
{
    public record ProcessInvestmentCommand(int InvestmentId, bool Accept = true) : IRequest;

    public class ProcessInvestmentCommandHandler : IRequestHandler<ProcessInvestmentCommand>
    {
        private readonly IApplicationDbContext _context;

        public ProcessInvestmentCommandHandler(IApplicationDbContext context)
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
            if (request.Accept)
            {
                investment.IsAccepted = true;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}