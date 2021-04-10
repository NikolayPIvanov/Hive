using System;
using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Investing;
using MediatR;

namespace Hive.Application.Investing.Plans.Commands
{
    public record CreatePlanCommand(int VendorId, string Title, string Description,
        int EstimatedReleaseDays, DateTime? EstimatedReleaseDate, decimal FundingNeeded) : IRequest<int>;
    
    public class CreatePlanCommandHandler : IRequestHandler<CreatePlanCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreatePlanCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<int> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
        {
            var plan = new Plan(request.VendorId, request.Title, request.Description, request.EstimatedReleaseDays,
                request.EstimatedReleaseDate, request.FundingNeeded);
            // TODO: Add validation
            _context.Plans.Add(plan);
            await _context.SaveChangesAsync(cancellationToken);

            return plan.Id;
        }
    }
}