using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Core;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Investing.Application.Plans.Commands
{
    public record UpdatePlanCommand(int Id, string Title, string Description, 
        DateTime StartDate, DateTime EndDate, decimal FundingNeeded, bool IsPublic) : IRequest;

    public class UpdatePlanCommandValidator : AbstractValidator<UpdatePlanCommand>
    {
        public UpdatePlanCommandValidator(IInvestingDbContext context)
        {
            RuleFor(x => x.Title)
                .MinimumLength(5).WithMessage("{Property} must be above {MinimumLength}")
                .MaximumLength(50).WithMessage("{Property} must be below {MaximumLength}")
                .NotEmpty().WithMessage("{Property} cannot be empty or missing");
            
            RuleFor(x => x.Description)
                .MinimumLength(20).WithMessage("{Property} must be above {MinimumLength}")
                .MaximumLength(3000).WithMessage("{Property} must be below {MaximumLength}")
                .NotEmpty().WithMessage("{Property} cannot be empty or missing");

            RuleFor(x => x.StartDate)
                .Must(x => DateTime.UtcNow.AddYears(1) > x).WithMessage("{Property} must not be more than an year away.")
                .NotEmpty().WithMessage("{Property} cannot be empty or missing");
            
            RuleFor(x => x.EndDate)
                .Must(x => DateTime.UtcNow.AddYears(5) > x).WithMessage("{Property} must not be more than 5 years away.")
                .NotEmpty().WithMessage("{Property} cannot be empty or missing");

            RuleFor(x => x.FundingNeeded)
                .InclusiveBetween(100.0m, 100000.0m).WithMessage("{Property} must between 100.0 and 100000.0")
                .NotEmpty().WithMessage("{Property} cannot be empty or missing");

            RuleFor(x => x.Id)
                .MustAsync(async (id, cancellationToken) =>
                    (await context.Plans.AnyAsync(x => x.Id == id && !x.IsFunded, cancellationToken)))
                .WithMessage("Plan that is already funded cannot be updated.");
        }
    }

    public class UpdatePlanCommandHandler : AuthorizationRequestHandler<Plan>, IRequestHandler<UpdatePlanCommand>
    {
        private readonly IInvestingDbContext _context;
        private readonly ILogger<UpdatePlanCommand> _logger;

        public UpdatePlanCommandHandler(IInvestingDbContext context, IAuthorizationService authorizationService, 
            ICurrentUserService currentUserService, ILogger<UpdatePlanCommand> logger) : base(currentUserService, authorizationService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(UpdatePlanCommand request, CancellationToken cancellationToken)
        {
            var plan = await _context.Plans.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            
            if (plan is null)
            {
                _logger.LogWarning("Plan with id: {Id} was not found", request.Id);
                throw new NotFoundException(nameof(Plan), request.Id);
            }

            var result = await base.AuthorizeAsync(plan,  new [] {"OnlyOwnerPolicy"});
            
            if (!result.All(s => s.Succeeded))
            {
                throw new ForbiddenAccessException();
            }
            
            plan.Title = request.Title;
            plan.Description = request.Description;
            plan.StartDate = request.StartDate;
            plan.EndDate = request.EndDate;
            plan.StartingFunds = request.FundingNeeded;
            plan.IsPublic = request.IsPublic;
            
            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Plan with id: {Id} was updated.", request.Id);

            return Unit.Value;
        }
    }
}