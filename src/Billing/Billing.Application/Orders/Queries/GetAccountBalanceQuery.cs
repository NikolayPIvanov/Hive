using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using Billing.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Billing.Application.Orders
{
    public record GetAccountBalanceQuery : IRequest<decimal>;

    public class GetAccountBalanceQueryHandler : IRequestHandler<GetAccountBalanceQuery, decimal>
    {
        private readonly IBillingContext _context;

        public GetAccountBalanceQueryHandler(IBillingContext context)
        {
            _context = context;
        }
        
        public async Task<decimal> Handle(GetAccountBalanceQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = "str";
            
            // Need to implement uplock
            var account = await _context.AccountHolders
                .Select(ah => new
                {
                    ah.UserId,
                    ah.Account.Transactions,
                })
                .FirstOrDefaultAsync(a => a.UserId == currentUserId, cancellationToken);
            
            var transactions = account.Transactions;

            // This needs to go into a snapshot
            var balance = 0.0m;
            foreach (var transaction in transactions)
            {
                switch (transaction.TransactionType)
                {
                    case TransactionType.Deposit: balance += transaction.Amount;
                        break;
                    case TransactionType.Hold: balance -= transaction.Amount;
                        break;
                    case TransactionType.Payment: balance -= transaction.Amount;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            if (balance < 0)
            {
                throw new Exception("Balance cannot be under 0.");
            }

            return balance;
        }
    }
}