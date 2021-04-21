using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Billing.Application.Interfaces;
using Hive.Billing.Domain.Entities;
using Hive.Billing.Domain.Enums;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Mappings;
using Hive.Common.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Billing.Application.Transactions.Queries
{
    public record TransactionDto(int PublicId, decimal Amount, TransactionType TransactionType, int WalletId,
        Guid? OrderNumber);
    
    public record GetTransactionsByWalletQuery(int WalletId, int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedList<TransactionDto>>;

    public class GetTransactionsByWalletQueryHandler : IRequestHandler<GetTransactionsByWalletQuery, PaginatedList<TransactionDto>>
    {
        private readonly IBillingDbContext _context;
        private readonly IMapper _mapper;

        public GetTransactionsByWalletQueryHandler(IBillingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<PaginatedList<TransactionDto>> Handle(GetTransactionsByWalletQuery request, CancellationToken cancellationToken)
        {
            var wallet = await _context.Wallets
                .Include(w => w.Transactions)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.WalletId, cancellationToken);

            if (wallet == null)
            {
                throw new NotFoundException(nameof(Wallet), request.WalletId);
            }

            return await wallet.Transactions
                .AsQueryable()
                .ProjectTo<TransactionDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}