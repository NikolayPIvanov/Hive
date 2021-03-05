using System.Collections.Generic;
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
    { }

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
            return await _context.Categories.ProjectToListAsync<CategoryDto>(_mapper.ConfigurationProvider);
        }
    }
}