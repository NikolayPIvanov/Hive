using System;
using System.Threading;
using System.Threading.Tasks;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using MediatR;

namespace Hive.Investing.Application.Plans.Commands
{
    public record CreatePlanCommand(int VendorId, string Title, string Description,
        int EstimatedReleaseDays, DateTime? EstimatedReleaseDate, decimal FundingNeeded) : IRequest<int>;
    
    public class CreatePlanCommandHandler : IRequestHandler<CreatePlanCommand, int>
    {
        private readonly IInvestingDbContext _context;

        public CreatePlanCommandHandler(IInvestingDbContext context)
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