using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Mappings;
using Hive.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.GigsManagement.Gigs.Queries.GetCategoryGigs
{
    public record GetCategoryGigsQuery(int CategoryId, int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedList<GigDto>>;

    public class GetCategoryGigsQueryHandler : IRequestHandler<GetCategoryGigsQuery, PaginatedList<GigDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetCategoryGigsQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        public async Task<PaginatedList<GigDto>> Handle(GetCategoryGigsQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Gigs
                .Include(g => g.GigScope)
                .Include(g => g.Tags)
                .Include(g => g.Category)
                .Where(g => g.CategoryId == request.CategoryId && !g.IsDraft)
                .ProjectTo<GigDto>(_mapper.ConfigurationProvider) 
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}