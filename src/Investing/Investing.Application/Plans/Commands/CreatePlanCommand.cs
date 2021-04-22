using System;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Security;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Investing.Application.Plans.Commands
{
    [Authorize(Roles = "Seller, Administrator")]
    public record CreatePlanCommand(string Title, string Description,
        int EstimatedReleaseDays, DateTime? EstimatedReleaseDate, decimal FundingNeeded) : IRequest<int>;
    
    // TODO: Add validation
    public class CreatePlanCommandHandler : IRequestHandler<CreatePlanCommand, int>
    {
        private readonly IInvestingDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreatePlanCommandHandler(IInvestingDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        
        public async Task<int> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
        {
            var vendor =
                await _context.Vendors.FirstOrDefaultAsync(x => x.UserId == _currentUserService.UserId,
                    cancellationToken);

            if (vendor == null)
            {
                throw new NotFoundException(nameof(Vendor), _currentUserService.UserId);
            }
            
            var plan = new Plan(vendor.Id, request.Title, request.Description, request.EstimatedReleaseDays,
                request.EstimatedReleaseDate, request.FundingNeeded);
            _context.Plans.Add(plan);
            await _context.SaveChangesAsync(cancellationToken);

            return plan.Id;
        }
    }
}