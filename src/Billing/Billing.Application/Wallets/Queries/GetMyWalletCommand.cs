using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Billing.Application.Interfaces;
using Hive.Billing.Domain.Entities;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Billing.Application.Wallets.Queries
{
    public record GetMyWalletCommand : IRequest<WalletDto>;

    public class GetMyWalletCommandHandler : IRequestHandler<GetMyWalletCommand, WalletDto>
    {
        private readonly IBillingDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<GetMyWalletCommandHandler> _logger;

        public GetMyWalletCommandHandler(IBillingDbContext context, IMapper mapper, ICurrentUserService currentUserService,
            ILogger<GetMyWalletCommandHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<WalletDto> Handle(GetMyWalletCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _context.Wallets
                .Include(w => w.AccountHolder)
                .Include(w => w.Transactions)
                .FirstOrDefaultAsync(x => x.AccountHolder.UserId == _currentUserService.UserId, cancellationToken);

            if (wallet == null)
            {
                _logger.LogWarning("Wallet for user with id: {@Id} was not found.", _currentUserService.UserId);
                throw new NotFoundException(nameof(Wallet));
            }
            
            return _mapper.Map<WalletDto>(wallet);
        }
    }
}