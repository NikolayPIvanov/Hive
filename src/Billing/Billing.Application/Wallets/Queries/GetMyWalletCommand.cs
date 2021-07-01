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
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Billing.Application.Wallets.Queries
{
    public record GetMyWalletCommand(int PageSize = 5) : IRequest<WalletDto>;

    public class GetMyWalletCommandHandler : IRequestHandler<GetMyWalletCommand, WalletDto>
    {
        private readonly IBillingDbContext _context;
        private readonly IRedisCacheClient _cacheClient;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<GetMyWalletCommandHandler> _logger;

        public GetMyWalletCommandHandler(IBillingDbContext context, 
            IRedisCacheClient cacheClient,
            IMapper mapper, 
            ICurrentUserService currentUserService,
            ILogger<GetMyWalletCommandHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _cacheClient = cacheClient;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<WalletDto> Handle(GetMyWalletCommand request, CancellationToken cancellationToken)
        {
            var query = _context.Wallets
                .AsNoTracking()
                .Include(w => w.AccountHolder)
                .Include(w => w.Transactions.OrderByDescending(x => x.Created).Take(request.PageSize));

            var cacheKey = $"wallet:{_currentUserService.UserId}";
            var walletId = await _cacheClient.GetDbFromConfiguration().GetAsync<int?>(cacheKey);

            Wallet? wallet = null;
            if (walletId != null)
            {
                wallet = await query.FirstOrDefaultAsync(w => w.Id == walletId.Value, cancellationToken: cancellationToken);
            }
            else
            {
                wallet = await query.FirstOrDefaultAsync(w => w.AccountHolder.UserId == _currentUserService.UserId, cancellationToken);
            }

            if (wallet == null)
            {
                _logger.LogWarning("Wallet for user with id: {@Id} was not found.", _currentUserService.UserId);
                throw new NotFoundException(nameof(Wallet));
            }

            if (walletId == null && await _cacheClient.GetDbFromConfiguration().AddAsync(cacheKey, wallet.Id, When.NotExists))
            {
                _logger.LogInformation($"Set wallet id={wallet.Id} in cache");
            }
            
            return _mapper.Map<WalletDto>(wallet);
        }
    }
}