using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Investing.Application.Investors.Queries
{
    public record GetInvestorIdQuery : IRequest<int>;

    public class GetInvestorIdQueryHandler : IRequestHandler<GetInvestorIdQuery, int>
    {
        private readonly IInvestingDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<GetInvestorIdQueryHandler> _logger;

        public GetInvestorIdQueryHandler(IInvestingDbContext context, ICurrentUserService currentUserService, ILogger<GetInvestorIdQueryHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(context));
            _logger = logger  ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<int> Handle(GetInvestorIdQuery request, CancellationToken cancellationToken)
        {
            var investor = await _context.Investors.Select(x => new {x.Id, x.UserId})
                .FirstOrDefaultAsync(x => x.UserId == _currentUserService.UserId, cancellationToken);
            
            if (investor == null)
            {
                _logger.LogWarning("Investor with user id: {UserId} was not found", _currentUserService.UserId);
                throw new NotFoundException(nameof(Investor));
            }

            return investor.Id;
        }
    }
    
}