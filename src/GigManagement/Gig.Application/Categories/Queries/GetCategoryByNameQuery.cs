using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Mappings;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.Categories.Queries
{
    public record GetCategoryByNameQuery(string Name) : IRequest<IEnumerable<CategoryDto>>;
    
    public class GetCategoryByNameQueryHandler : IRequestHandler<GetCategoryByNameQuery, IEnumerable<CategoryDto>>
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCategoryQueryHandler> _logger;

        public GetCategoryByNameQueryHandler(IGigManagementDbContext dbContext, IMapper mapper, ILogger<GetCategoryQueryHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<IEnumerable<CategoryDto>> Handle(GetCategoryByNameQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Categories
                .Where(x => x.Title.ToLower().Contains(request.Name))
                .ProjectToListAsync<CategoryDto>(_mapper.ConfigurationProvider);
        }
    }
}