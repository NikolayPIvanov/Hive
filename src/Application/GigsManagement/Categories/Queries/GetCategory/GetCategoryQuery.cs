using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Gigs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Application.Categories.Queries.GetCategory
{
    public record GetCategoryQuery(int Id) : IRequest<CategoryDto>;

    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCategoryQueryHandler> _logger;

        public GetCategoryQueryHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<GetCategoryQueryHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<CategoryDto> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (category == null)
            {
                _logger.LogError("Category with {@Id} was not found.", request.Id);
                throw new NotFoundException(nameof(Category), request.Id);
            }

            var dto = _mapper.Map<CategoryDto>(category);
            return dto;
        }
    }
}