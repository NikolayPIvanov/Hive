using System;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Exceptions;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Investing.Application.Plans.Commands
{
    public record UpdatePlanCommand(int Id, string Title, string Description,
        int EstimatedReleaseDays, DateTime? EstimatedReleaseDate, decimal FundingNeeded) : IRequest;
    
    public class UpdatePlanCommandHandler : IRequestHandler<UpdatePlanCommand>
    {
        private readonly IInvestingDbContext _context;

        public UpdatePlanCommandHandler(IInvestingDbContext context)
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