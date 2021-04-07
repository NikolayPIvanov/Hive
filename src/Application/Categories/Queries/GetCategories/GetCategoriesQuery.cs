using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hive.Application.Categories.Queries.GetCategory;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Mappings;
using Hive.Application.Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hive.Application.Categories.Queries.GetCategories
{
    public record GetCategoriesQuery(int PageNumber = 1, int PageSize = 10, bool OnlyParents = false) : IRequest<PaginatedList<CategoryDto>>;

    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, PaginatedList<CategoryDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCategoriesQueryHandler> _logger;

        public GetCategoriesQueryHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetCategoriesQueryHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<PaginatedList<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Categories.AsQueryable();
            var (pageNumber, pageSize, onlyParents) = request;
            if (onlyParents)
            {
                query = query.Where(c => !c.ParentId.HasValue);
            }

            var categories = await query
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(pageNumber, pageSize);
            
            _logger.LogInformation("Successfully executed query for categories - {@PageNumber} {@PageSize} {@OnlyParents}", pageNumber, pageSize, onlyParents);
            
            return categories;
        }
    }
}