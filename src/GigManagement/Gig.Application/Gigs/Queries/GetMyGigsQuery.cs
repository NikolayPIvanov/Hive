using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Mappings;
using Hive.Common.Core.Security;
using Hive.Gig.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.Gigs.Queries
{
    public record GetMyGigsQuery : IRequest<IEnumerable<GigDto>>;

    public class GetMyGigsQueryHandler : IRequestHandler<GetMyGigsQuery, IEnumerable<GigDto>>
    {
        private readonly IGigManagementDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ILogger<GetMyGigsQueryHandler> _logger;

        public GetMyGigsQueryHandler(IGigManagementDbContext context, ICurrentUserService currentUserService, IMapper mapper, ILogger<GetMyGigsQueryHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }
        
        public async Task<IEnumerable<GigDto>> Handle(GetMyGigsQuery request, CancellationToken cancellationToken)
        {
            var seller = await _context.Sellers
                .Select(x => new { x.Id, x.UserId })
                .FirstOrDefaultAsync(s => s.UserId == _currentUserService.UserId, cancellationToken);

            if (seller == null)
            {
                _logger.LogWarning($"Seller Account does not exist for {_currentUserService.UserId}");
                throw new NotFoundException($"Seller Account does not exist for {_currentUserService.UserId}");
            }
            
            var query = _context.Gigs.AsNoTracking().Where(g => g.SellerId == seller.Id);
            return await query.ProjectToListAsync<GigDto>(_mapper.ConfigurationProvider);
        }
    }
}