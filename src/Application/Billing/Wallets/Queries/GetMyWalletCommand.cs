using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Billing.Transactions.Queries;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Billing;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Billing.Wallets.Queries
{
    public class WalletDto
    {
        public int WalletId { get; set; }
        
        public ICollection<TransactionDto> Transactions { get; set; }
    }

    public record GetMyWalletCommand : IRequest<WalletDto>;

    public class GetMyWalletCommandHandler : IRequestHandler<GetMyWalletCommand, WalletDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetMyWalletCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        
        public async Task<WalletDto> Handle(GetMyWalletCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _context.AccountHolders
                .Select(x => new
                {
                    x.UserId,
                    x.Wallet.Transactions,
                    x.Wallet
                })
                .FirstOrDefaultAsync(x => x.UserId == _currentUserService.UserId, cancellationToken: cancellationToken);

            if (wallet == null)
            {
                throw new NotFoundException(nameof(Wallet));
            }

            return new WalletDto
            {
                Transactions = _mapper.Map<ICollection<TransactionDto>>(wallet.Transactions),
                WalletId = wallet.Wallet.Id
            };
        }
    }
}