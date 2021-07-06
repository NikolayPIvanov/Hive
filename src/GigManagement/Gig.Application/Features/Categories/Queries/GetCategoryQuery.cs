﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Contracts.Objects;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.Features.Categories.Queries
{
    public record GetCategoryQuery(int Id) : IRequest<CategoryDto>;

    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryDto>
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCategoryQueryHandler> _logger;

        public GetCategoryQueryHandler(IGigManagementDbContext dbContext, IMapper mapper, ILogger<GetCategoryQueryHandler> logger)
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