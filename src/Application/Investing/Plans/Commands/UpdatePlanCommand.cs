using System;
using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Security;
using Hive.Domain.Entities.Investing;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Investing.Plans.Commands
{
    [Authorize(Roles = "Seller, Administrator")]
    public record UpdatePlanCommand(int Id, string Title, string Description,
        int EstimatedReleaseDays, DateTime? EstimatedReleaseDate, decimal FundingNeeded) : IRequest;
    
    public class UpdatePlanCommandHandler : IRequestHandler<UpdatePlanCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdatePlanCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(UpdatePlanCommand request, CancellationToken cancellationToken)
        {
            var plan = await _context.Plans.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            
            if (plan is null)
            {
                throw new NotFoundException(nameof(Plan), request.Id);
            }
            
            // TODO: Add validation
            plan.Title = request.Title;
            plan.Description = request.Description;
            plan.EstimatedReleaseDate = request.EstimatedReleaseDate;
            plan.EstimatedReleaseDays = request.EstimatedReleaseDays;
            plan.FundingNeeded = plan.FundingNeeded;
            
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}