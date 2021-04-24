using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Security;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Investing.Application.Plans.Commands
{
    public record CreatePlanCommand(string Title, string Description,
        DateTime StartDate, DateTime EndDate, decimal FundingNeeded) : IRequest<int>;

    public class CreatePlanCommandValidator : AbstractValidator<CreatePlanCommand>
    {
        public CreatePlanCommandValidator()
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
        }
    }
    
    public class CreatePlanCommandHandler : IRequestHandler<CreatePlanCommand, int>
    {
        private readonly IInvestingDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<CreatePlanCommandHandler> _logger;

        public CreatePlanCommandHandler(IInvestingDbContext context, ICurrentUserService currentUserService,
            ILogger<CreatePlanCommandHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<int> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
        {
            var vendor = await _context.Vendors.FirstOrDefaultAsync(x => x.UserId == _currentUserService.UserId, cancellationToken);

            if (vendor == null)
            {
                _logger.LogWarning("Vendor with user id: {UserId} was not found", _currentUserService.UserId);
                throw new NotFoundException(nameof(Vendor), _currentUserService.UserId);
            }
            
            var plan = new Plan(vendor.Id, request.Title, request.Description,
                request.StartDate, request.EndDate, request.FundingNeeded);
            _context.Plans.Add(plan);
            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Plan {Id} was created", plan.Id);

            return plan.Id;
        }
    }
}