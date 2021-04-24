using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Investing.Application.Plans.Commands
{
    public record DeletePlanCommand(int Id) : IRequest;

    public class DeletePlanCommandValidator : AbstractValidator<DeletePlanCommand>
    {
        public DeletePlanCommandValidator(IInvestingDbContext context)
        {
            RuleFor(x => x.Id)
                .MustAsync(async (id, token) => 
                    !(await context.Plans.AnyAsync(x => x.Id == id && x.EndDate > DateTime.UtcNow, token)))
                .WithMessage("Cannot delete a plan that is still ongoing");
        }
    }

    public class DeletePlanCommandHandler : IRequestHandler<DeletePlanCommand>
    {
        private readonly IInvestingDbContext _context;
        private readonly ILogger<DeletePlanCommandHandler> _logger;

        public DeletePlanCommandHandler(IInvestingDbContext context, ILogger<DeletePlanCommandHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(DeletePlanCommand request, CancellationToken cancellationToken)
        {
            var plan = await _context.Plans.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            
            if (plan is null)
            {
                _logger.LogWarning("Plan with id {Id} was not found", request.Id);
                throw new NotFoundException(nameof(Plan), request.Id);
            }

            _context.Plans.Remove(plan);
            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Plan with id {Id} was deleted", request.Id);

            
            return Unit.Value;
        }
    }
}