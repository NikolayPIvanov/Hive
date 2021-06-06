using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Billing.Application.Interfaces;
using Hive.Billing.Domain.Entities;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Mappings;
using Hive.Common.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Billing.Application.Wallets.Queries
{
    public record WalletDto(int Id, int AccountHolderId, decimal Balance, ICollection<TransactionDto> Transactions);
    
    public record GetWalletTransactionsQuery(int WalletId, int PageNumber = 1, int PageSize = 5) : IRequest<PaginatedList<TransactionDto>>;

    public class GetWalletTransactionsQueryHandler : IRequestHandler<GetWalletTransactionsQuery, PaginatedList<TransactionDto>>
    {
        private readonly IBillingDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetWalletTransactionsQueryHandler> _logger;

        public GetWalletTransactionsQueryHandler(IBillingDbContext context, IMapper mapper, ILogger<GetWalletTransactionsQueryHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<PaginatedList<TransactionDto>> Handle(GetWalletTransactionsQuery request, CancellationToken cancellationToken)
        {
            var walletExists = await _context.Wallets.AnyAsync(w => w.Id == request.WalletId, cancellationToken);

            if (!walletExists)
            {
                _logger.LogWarning("Wallet with id: {@Id} was not found.", request.WalletId);
                throw new NotFoundException(nameof(Wallet), request.WalletId);
            }

            return await _context.Transactions
                .Where(t => t.WalletId == request.WalletId)
                .ProjectTo<TransactionDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}