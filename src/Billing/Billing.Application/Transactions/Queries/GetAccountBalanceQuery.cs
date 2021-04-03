using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using Hive.Billing.Domain.Entities;
using Hive.Billing.Domain.Enums;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Billing.Application.Transactions.Queries
{
    public record GetAccountBalanceQuery : IRequest<decimal>;

    public class GetAccountBalanceQueryHandler : IRequestHandler<GetAccountBalanceQuery, decimal>
    {
        private readonly IBillingDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<GetAccountBalanceQueryHandler> _logger;

        public GetAccountBalanceQueryHandler(IBillingDbContext context, ICurrentUserService currentUserService, ILogger<GetAccountBalanceQueryHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _logger = logger ?? throw new ArgumentNullException(nameof(context));
        }
        
        public async Task<decimal> Handle(GetAccountBalanceQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
            
            // Need to implement uplock
            var account = await _context.AccountHolders
                .Select(ah => new
                {
                    ah.UserId,
                    ah.Account.DefaultPaymentMethodId,
                })
                .FirstOrDefaultAsync(a => a.UserId == currentUserId, cancellationToken);
            
            if (account is null)
            {
                _logger.LogWarning("Account for the logged in user {@UserId} has not been found.", currentUserId);
                throw new NotFoundException(nameof(Account));
            }
           
            // TODO: this should be in validation
            if (!account.DefaultPaymentMethodId.HasValue)
            {
                _logger.LogWarning("Default method for the logged in user {@UserId} account has not been found.", currentUserId);
                throw new NotFoundException(nameof(Account));
            }
            
            var transactions =
                await _context.Transactions
                    .Where(t => t.PaymentMethodId == account.DefaultPaymentMethodId.Value)
                    .ToListAsync(cancellationToken: cancellationToken);

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

            var lowerThreshold = 0.0m;
            if (balance < lowerThreshold)
            {
                _logger.LogWarning("Account balance for {@AccountHolder} is below {@LowerThreshold}", account.UserId, lowerThreshold);
            }

            return balance;
        }
    }
}