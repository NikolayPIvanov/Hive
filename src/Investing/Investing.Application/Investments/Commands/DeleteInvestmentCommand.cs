using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using MediatR;

namespace Hive.Investing.Application.Investments.Commands
{
    public record DeleteInvestmentCommand(int Id) : IRequest;

    public class DeleteInvestmentCommandValidator : AbstractValidator<DeleteInvestmentCommand>
    {
        public DeleteInvestmentCommandValidator(IInvestingDbContext context)
        {
            RuleFor(x => x.Id)
                .MustAsync(async (id, token) =>
                {
                    var investment = await context.Investments.FindAsync(id);
                    if (investment == null) return true; // need to raise not found

                    return !investment.IsAccepted;
                }).WithMessage("Cannot delete an investment that is already accepted by the vendor.");
        }
    }
    
    public class DeleteInvestmentCommandHandler : IRequestHandler<DeleteInvestmentCommand>
    {
        private readonly IInvestingDbContext _context;

        public DeleteInvestmentCommandHandler(IInvestingDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        public async Task<Unit> Handle(DeleteInvestmentCommand request, CancellationToken cancellationToken)
        {
            var investment = await _context.Investments.FindAsync(request.Id);
            if (investment == null)
            {
                throw new NotFoundException(nameof(Investment), request.Id);
            }

            _context.Investments.Remove(investment);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}