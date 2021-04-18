using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Mappings;
using Hive.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.GigsManagement.Gigs.Queries.GetMyGigs
{
    [Authorize(Roles = "Seller")]
    public record GetMyGigsQuery : IRequest<IEnumerable<GigDto>>;

    public class GetMyGigsQueryHandler : IRequestHandler<GetMyGigsQuery, IEnumerable<GigDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetMyGigsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
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
            
            var query = _context.Gigs.AsNoTracking().Where(g => g.SellerId == seller.Id);
            return await query.ProjectToListAsync<GigDto>(_mapper.ConfigurationProvider);
        }
    }
}