using System;
using System.Threading;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using FluentValidation;
using Hive.Billing.Domain.Entities;
using Hive.Billing.Domain.Enums;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Billing.Application.Transactions.Commands
{
    [Authorize(Roles = "Buyer, Administrator")]
    public record CreateTransactionCommand(decimal Amount, Guid? OrderNumber, int WalletId, TransactionType TransactionType = TransactionType.Fund) : IRequest<int>;

    public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionCommandValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0.0m).WithMessage("Must be above 0.0");
        }
    }
    
    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, int>
    {
        private readonly IBillingDbContext _context;

        public CreateTransactionCommandHandler(IBillingDbContext context)
        {
            _context = context;
        }
        
        public async Task<int> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var walletExists = await _context.Wallets.AnyAsync(w => w.Id == request.WalletId, cancellationToken);
            if (!walletExists)
            {
                throw new NotFoundException(nameof(Wallet), request.WalletId);
            }
            
            var transaction = new Transaction(request.Amount, request.OrderNumber, request.TransactionType, request.WalletId);

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync(cancellationToken);

            return transaction.PublicId;
        }
    }
}