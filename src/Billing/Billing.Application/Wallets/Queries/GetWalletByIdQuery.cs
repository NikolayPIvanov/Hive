using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Billing.Application.Interfaces;
using Hive.Billing.Domain.Entities;
using Hive.Common.Core.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Billing.Application.Wallets.Queries
{
    public record WalletDto(int Id, decimal Balance, ICollection<TransactionDto> Transactions);
    
    public record GetWalletByIdQuery(int WalletId) : IRequest<WalletDto>;

    public class GetWalletByIdQueryHandler : IRequestHandler<GetWalletByIdQuery, WalletDto>
    {
        private readonly IBillingDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetWalletByIdQueryHandler> _logger;

        public GetWalletByIdQueryHandler(IBillingDbContext context, IMapper mapper, ILogger<GetWalletByIdQueryHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<WalletDto> Handle(GetWalletByIdQuery request, CancellationToken cancellationToken)
        {
            var wallet = await _context.Wallets
                .Include(w => w.Transactions)
                .FirstOrDefaultAsync(x => x.Id == request.WalletId, cancellationToken);

            if (wallet == null)
            {
                _logger.LogWarning("Wallet with id: {@Id} was not found.", request.WalletId);
                throw new NotFoundException(nameof(Wallet), request.WalletId);
            }

            return _mapper.Map<WalletDto>(wallet);
        }
    }
}