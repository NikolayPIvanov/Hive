using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Security;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Investing.Application.Investors.Queries
{
    [Authorize(Roles = "Investor")]
    public record GetInvestorIdQuery : IRequest<int>;

    public class GetInvestorIdQueryHandler : IRequestHandler<GetInvestorIdQuery, int>
    {
        private readonly IInvestingDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetInvestorIdQueryHandler(IInvestingDbContext context, ICurrentUserService currentUserService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(context));
        }
        
        public async Task<int> Handle(GetInvestorIdQuery request, CancellationToken cancellationToken)
        {
            var investor = await _context.Investors.Select(x => new {x.Id, x.UserId})
                .FirstOrDefaultAsync(x => x.UserId == _currentUserService.UserId, cancellationToken);
            
            if (investor == null)
            {
                throw new NotFoundException(nameof(Investor));
            }

            return investor.Id;
        }
    }
    
}