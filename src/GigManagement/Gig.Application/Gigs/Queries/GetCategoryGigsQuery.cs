using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hive.Common.Core.Mappings;
using Hive.Common.Core.Models;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Contracts.Objects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Gigs.Queries
{
    public record GetCategoryGigsQuery(int CategoryId) : PaginatedQuery, IRequest<PaginatedList<GigDto>>;

    public class GetCategoryGigsQueryHandler : IRequestHandler<GetCategoryGigsQuery, PaginatedList<GigDto>>
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetCategoryGigsQueryHandler(IGigManagementDbContext dbContext, IMapper mapper)
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