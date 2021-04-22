using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Security;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Investing.Application.Plans.Commands
{
    [Authorize(Roles = "Seller, Administrator")]
    public record DeletePlanCommand(int Id) : IRequest;

    public class DeletePlanCommandHandler : IRequestHandler<DeletePlanCommand>
    {
        private readonly IInvestingDbContext _context;

        public DeletePlanCommandHandler(IInvestingDbContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(DeletePlanCommand request, CancellationToken cancellationToken)
        {
            var plan = await _context.Plans.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            
            if (plan is null)
            {
                throw new NotFoundException(nameof(Plan), request.Id);
            }

            _context.Plans.Remove(plan);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}