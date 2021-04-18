using System;
using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Gigs;
using Hive.Domain.Entities.Investing;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Investing.Plans.Commands
{
    public record CreatePlanCommand(string Title, string Description,
        int EstimatedReleaseDays, DateTime? EstimatedReleaseDate, decimal FundingNeeded) : IRequest<int>;
    
    public class CreatePlanCommandHandler : IRequestHandler<CreatePlanCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreatePlanCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        
        public async Task<int> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
        {
            var seller =
                await _context.Sellers.FirstOrDefaultAsync(x => x.UserId == _currentUserService.UserId,
                    cancellationToken);

            if (seller == null)
            {
                throw new NotFoundException(nameof(Seller), _currentUserService.UserId);
            }
            
            var plan = new Plan(seller.Id, request.Title, request.Description, request.EstimatedReleaseDays,
                request.EstimatedReleaseDate, request.FundingNeeded);
            // TODO: Add validation
            _context.Plans.Add(plan);
            await _context.SaveChangesAsync(cancellationToken);

            return plan.Id;
        }
    }
}