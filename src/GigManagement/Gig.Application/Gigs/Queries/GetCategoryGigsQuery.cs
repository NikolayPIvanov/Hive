using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hive.Common.Application.Mappings;
using Hive.Common.Application.Models;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Contracts.Objects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Gigs.Queries
{
    public record GetCategoryGigsQuery(int CategoryId) : PaginatedQuery, IRequest<PaginatedList<GigDto>>;

    public class GetCategoryGigsQueryHandler : IRequestHandler<GetCategoryGigsQuery, PaginatedList<GigDto>>
    {
        private readonly IGigManagementContext _context;
        private readonly IMapper _mapper;

        public GetCategoryGigsQueryHandler(IGigManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<PaginatedList<GigDto>> Handle(GetCategoryGigsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Gigs
                .Include(g => g.GigScope)
                .Include(g => g.Tags)
                .Include(g => g.Category)
                .Where(g => g.CategoryId == request.CategoryId && !g.IsDraft)
                .ProjectTo<GigDto>(_mapper.ConfigurationProvider) 
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}