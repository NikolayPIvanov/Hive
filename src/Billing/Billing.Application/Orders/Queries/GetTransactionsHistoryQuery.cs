using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Billing.Application.Interfaces;
using Billing.Domain;
using Hive.Billing.Contracts.Objects;
using Hive.Common.Application.Exceptions;
using Hive.Common.Application.Mappings;
using Hive.Common.Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Billing.Application.Orders.Queries
{
    public record GetTransactionsHistoryQuery(int PageSize = 10, int PageNumber = 1) : IRequest<PaginatedList<TransactionDto>>;
    
    public class GetTransactionsHistoryQueryHandler : IRequestHandler<GetTransactionsHistoryQuery, PaginatedList<TransactionDto>>
    {
        private readonly IBillingContext _context;
        private readonly IMapper _mapper;

        public GetTransactionsHistoryQueryHandler(IBillingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<PaginatedList<TransactionDto>> Handle(GetTransactionsHistoryQuery request, CancellationToken cancellationToken)
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

            if (account is null)
            {
                throw new NotFoundException(nameof(AccountHolder), null);
            }
            
            return await account.Transactions
                .AsQueryable()
                .ProjectTo<TransactionDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}