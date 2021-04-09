using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.GigsManagement.Gigs.Queries.GetMyGigs
{
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
            var sellerUserId = _currentUserService.UserId;
            // check if seller and is valid
            var query = _context.Gigs.AsNoTracking();
            if (sellerUserId != null)
            {
                query = query.Where(g => g.SellerId == 1);
            }

            return await query.ProjectToListAsync<GigDto>(_mapper.ConfigurationProvider);
        }
    }
}