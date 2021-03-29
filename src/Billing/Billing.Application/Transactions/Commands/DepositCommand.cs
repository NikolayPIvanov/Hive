using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using Billing.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Billing.Application.Transactions.Commands
{
    public record DepositCommand(int PaymentMethodId, decimal Amount) : IRequest<Guid>;
    
    public class DepositCommandHandler : IRequestHandler<DepositCommand, Guid>
    {
        private readonly IBillingContext _context;
    
        public DepositCommandHandler(IBillingContext context)
        {
            _context = context;
        }
        
        public async Task<Guid> Handle(DepositCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = "str";
            var accountId =
                await _context.AccountHolders
                    .AsNoTracking()
                    .Select(x => new {x.UserId, x.AccountId})
                    .FirstOrDefaultAsync(x => x.UserId == currentUserId, cancellationToken);

            var transaction = new Transaction(TransactionType.Deposit, request.Amount, accountId.AccountId,
                request.PaymentMethodId, null);

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync(cancellationToken);

            return transaction.Id;
        }
    }
}