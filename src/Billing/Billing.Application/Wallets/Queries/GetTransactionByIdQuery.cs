using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Billing.Application.Interfaces;
using Hive.Billing.Domain.Entities;
using Hive.Billing.Domain.Enums;
using Hive.Common.Core.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Billing.Application.Wallets.Queries
{
    public record TransactionDto(int TransactionNumber, decimal Amount, TransactionType TransactionType, int WalletId,
        Guid? OrderNumber);
    
    public record GetTransactionByIdQuery(int TransactionNumber) : IRequest<TransactionDto>;

    public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, TransactionDto>
    {
        private readonly IBillingDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTransactionByIdQueryHandler> _logger;

        public GetTransactionByIdQueryHandler(IBillingDbContext context, IMapper mapper, ILogger<GetTransactionByIdQueryHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<TransactionDto> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var transaction =
                await _context.Transactions.FirstOrDefaultAsync(t => t.TransactionNumber == request.TransactionNumber, cancellationToken);

            if (transaction == null)
            {
                _logger.LogWarning("Transaction with TransactionNumber: {@TransactionNumber} was not found", request.TransactionNumber);
                throw new NotFoundException(nameof(Transaction), request.TransactionNumber);
            }

            return _mapper.Map<TransactionDto>(transaction);
        }
    }
}