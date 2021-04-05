using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using Hive.Billing.Domain;
using Hive.Billing.Domain.Entities;
using Hive.Billing.Domain.Enums;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Billing.Application.Transactions.Commands
{
    public record DepositCommand(int PaymentMethodId, decimal Amount) : IRequest<int>;
    
    public class DepositCommandHandler : IRequestHandler<DepositCommand, int>
    {
        private readonly IBillingDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<DepositCommandHandler> _logger;

        public DepositCommandHandler(IBillingDbContext context, ICurrentUserService currentUserService, ILogger<DepositCommandHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _logger = logger ?? throw new ArgumentNullException(nameof(context));
        }
        
        public async Task<int> Handle(DepositCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
            var account =
                await _context.AccountHolders
                    .AsNoTracking()
                    .Select(x => new {x.UserId, x.Account.DefaultPaymentMethodId})
                    .FirstOrDefaultAsync(x => x.UserId == currentUserId, cancellationToken);

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
            
            var transaction = new Transaction(currentUserId, TransactionType.Deposit, request.Amount, account.DefaultPaymentMethodId.Value, null);

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync(cancellationToken);

            return transaction.TransactionId;
        }
    }
}