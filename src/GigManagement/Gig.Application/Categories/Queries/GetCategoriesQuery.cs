using System.Collections.Generic;
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

namespace Hive.Gig.Application.Categories.Queries
{
    public record GetCategoriesQuery(int PageNumber = 1, int PageSize = 10, bool OnlyParents = false) : IRequest<PaginatedList<CategoryDto>>;

    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, PaginatedList<CategoryDto>>
    {
        private readonly IGigManagementContext _context;
        private readonly IMapper _mapper;

        public GetCategoriesQueryHandler(IGigManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<PaginatedList<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Categories.AsQueryable();
            if (request.OnlyParents)
            {
                query = query.Where(c => !c.ParentId.HasValue);
            }

            var categories = await query
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
            
            return categories;
        }
    }
}