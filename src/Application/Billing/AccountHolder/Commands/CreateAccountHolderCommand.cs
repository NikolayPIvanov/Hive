using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Billing.AccountHolder.Commands
{
    public record CreateAccountHolderCommand(string UserId) : IRequest<int>;

    public class CreateAccountHolderCommandValidator : AbstractValidator<CreateAccountHolderCommand>
    {
        public CreateAccountHolderCommandValidator(IApplicationDbContext context)
        {
            RuleFor(x => x.UserId)
                .MustAsync(async (id, token) => await context.AccountHolders.AnyAsync(x => x.UserId == id, token));
        }
    }

    public class CreateAccountHolderCommandHandler : IRequestHandler<CreateAccountHolderCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateAccountHolderCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<int> Handle(CreateAccountHolderCommand request, CancellationToken cancellationToken)
        {
            var accountHolder = new Domain.Entities.Billing.AccountHolder(request.UserId);

            _context.AccountHolders.Add(accountHolder);
            await _context.SaveChangesAsync(cancellationToken);

            return accountHolder.Id;
        }
    }
}