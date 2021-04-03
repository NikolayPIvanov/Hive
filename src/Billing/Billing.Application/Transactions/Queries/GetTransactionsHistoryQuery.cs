using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Billing.Application.Interfaces;
using Billing.Application.Objects;
using Hive.Billing.Domain.Entities;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Mappings;
using Hive.Common.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Billing.Application.Transactions.Queries
{
    public record GetTransactionsHistoryQuery(int? PaymentMethodId, int PageSize = 10, int PageNumber = 1) : IRequest<PaginatedList<TransactionDto>>;
    
    public class GetTransactionsHistoryQueryHandler : IRequestHandler<GetTransactionsHistoryQuery, PaginatedList<TransactionDto>>
    {
        private readonly IBillingDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTransactionsHistoryQueryHandler> _logger;

        public GetTransactionsHistoryQueryHandler(
            IBillingDbContext context, 
            ICurrentUserService currentUserService,
            IMapper mapper,
            ILogger<GetTransactionsHistoryQueryHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(context));
        }
        
        public async Task<PaginatedList<TransactionDto>> Handle(GetTransactionsHistoryQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
            
            // TODO: Need to implement uplock
            var account = await _context.AccountHolders
                .Select(ah => new
                {
                    ah.UserId,
                    ah.Account.PaymentMethods,
                    ah.Account.DefaultPaymentMethodId
                })
                .FirstOrDefaultAsync(a => a.UserId == currentUserId, cancellationToken);

            if (account is null)
            {
                _logger.LogWarning("Account holder with {@UserId} has not been found.", _currentUserService);
                throw new NotFoundException(nameof(AccountHolder), null);
            }
            
            var paymentMethodId = GetPaymentMethodId(account.DefaultPaymentMethodId, account.PaymentMethods,
                request.PaymentMethodId);

            if (paymentMethodId == null)
            {
                throw new NotFoundException(nameof(PaymentMethod));
            }

            var paymentMethod = await _context.PaymentMethods
                .Include(pm => pm.Transactions)
                .FirstAsync(x => x.Id == paymentMethodId.Value, cancellationToken: cancellationToken);
            
            return await paymentMethod.Transactions
                .AsQueryable()
                .ProjectTo<TransactionDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }

        private int? GetPaymentMethodId(int? defaultPaymentMethodId, IEnumerable<PaymentMethod> paymentMethods, int? requestPaymentMethodId)
        {
            if (defaultPaymentMethodId == null && requestPaymentMethodId == null)
            {
                _logger.LogWarning("Both request and default payment ids were not set.");
                return null;
            }

            return requestPaymentMethodId == null
                ? defaultPaymentMethodId.Value
                : paymentMethods?.FirstOrDefault(p => p.Id == requestPaymentMethodId.Value)?.Id;
        }
    }
}