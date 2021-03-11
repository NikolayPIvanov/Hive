using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Categories.Queries.GetCategory;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Mappings;
using MediatR;

namespace Hive.Application.Categories.Queries.GetCategories
{
    public class GetCategoriesQuery : IRequest<List<CategoryDto>>
    {
        public int? Limit { get; set; } = null;

        public bool OnlyParent { get; set; } = false;
    }

    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Categories.AsQueryable();
            if (request.OnlyParent)
            {
                query = query.Where(c => c.ParentCategoryId == null);
            }
            
            if (request.Limit.HasValue)
            {
                query = query.Take(request.Limit.Value);
            }

            query = query.OrderBy(c => c.Title);
            
            return await query.ProjectToListAsync<CategoryDto>(_mapper.ConfigurationProvider);
        }
    }
}