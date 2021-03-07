using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Mappings;
using Hive.Application.Common.Models;
using Hive.Application.Gigs.Queries.GetGig;
using MediatR;

namespace Hive.Application.Gigs.Queries.GetGigsForCategory
{
    public record GetGigsForCategoryQuery
        (int Id, int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedList<GigDto>>;
        
    public class GetGigsForCategoryQueryHandler : IRequestHandler<GetGigsForCategoryQuery, PaginatedList<GigDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetGigsForCategoryQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<PaginatedList<GigDto>> Handle(GetGigsForCategoryQuery request, CancellationToken cancellationToken)
        {
            var (id, pageNumber, pageSize) = request;
            return await _context.Gigs
                .Where(g => g.CategoryId == id)
                .OrderBy(x => x.Title)
                .ProjectTo<GigDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(pageNumber, pageSize); ;
        }
    }
}