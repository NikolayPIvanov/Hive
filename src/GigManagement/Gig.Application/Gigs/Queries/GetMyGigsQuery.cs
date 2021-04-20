using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Mappings;
using Hive.Common.Core.Security;
using Hive.Gig.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Gigs.Queries
{
    [Authorize(Roles = "Seller")]
    public record GetMyGigsQuery : IRequest<IEnumerable<GigDto>>;

    public class GetMyGigsQueryHandler : IRequestHandler<GetMyGigsQuery, IEnumerable<GigDto>>
    {
        private readonly IGigManagementDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetMyGigsQueryHandler(IGigManagementDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<GigDto>> Handle(GetMyGigsQuery request, CancellationToken cancellationToken)
        {
            var seller =
                await _context.Sellers.FirstOrDefaultAsync(s => s.UserId == _currentUserService.UserId,
                    cancellationToken);

            if (seller == null)
            {
                throw new Exception($"Seller Account does not exist for {_currentUserService.UserId}");
            }
            
            var query = _context.Gigs.AsNoTracking().Where(g => g.SellerId == seller.Id);
            return await query.ProjectToListAsync<GigDto>(_mapper.ConfigurationProvider);
        }
    }
}