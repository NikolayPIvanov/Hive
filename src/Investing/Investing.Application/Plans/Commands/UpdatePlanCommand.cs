using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Security;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Investing.Application.Plans.Commands
{
    [Authorize(Roles = "Seller, Administrator")]
    public record UpdatePlanCommand(int Id, string Title, string Description, DateTime EstimatedReleaseDate, decimal FundingNeeded) : IRequest;

    public class UpdatePlanCommandValidator : AbstractValidator<UpdatePlanCommand>
    {
        public UpdatePlanCommandValidator(IInvestingDbContext context)
        {
            RuleFor(x => x.Title)
                .NotNull().WithMessage("A {PropertyName} must be provided")
                .MinimumLength(3).WithMessage("{PropertyName} must be above {MinimumLength}")
                .MaximumLength(50).WithMessage("{PropertyName} must be below {MaximumLength}");
            
            RuleFor(x => x.Description)
                .NotNull().WithMessage("A {PropertyName} must be provided")
                .MinimumLength(10).WithMessage("{PropertyName} must be above {MinimumLength}")
                .MaximumLength(2500).WithMessage("{PropertyName} must be below {MaximumLength}");

            RuleFor(x => x.EstimatedReleaseDate.Date)
                .NotNull().WithMessage("A {PropertyName} must be provided")
                .GreaterThan(DateTime.UtcNow.Date).WithMessage("Cannot set release date to be today's date");

            RuleFor(x => x.FundingNeeded)
                .NotNull().WithMessage("A {PropertyName} must be provided")
                .GreaterThan(10.0m).WithMessage("{PropertyName} must be greater than {ValueToCompare}");

            RuleFor(x => x.Id)
                .MustAsync(async (command, id, cancellationToken) =>
                {
                    var alreadyFunded = await context.Plans.AnyAsync(x => x.Id == id && x.IsFunded, cancellationToken);
                    return !alreadyFunded;
                });
        }
    }
    
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
            plan.FundingNeeded = plan.FundingNeeded;
            
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}