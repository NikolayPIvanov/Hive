﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hive.Common.Core.Mappings;
using Hive.Common.Core.Models;
using Hive.Gig.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.Gigs.Queries
{
    public record GetCategoryGigsQuery(int CategoryId, int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedList<GigOverviewDto>>;
    
    public class GetCategoryGigsQueryHandler : IRequestHandler<GetCategoryGigsQuery, PaginatedList<GigOverviewDto>>
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCategoryGigsQueryHandler> _logger;

        public GetCategoryGigsQueryHandler(IGigManagementDbContext dbContext, IMapper mapper, ILogger<GetCategoryGigsQueryHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<PaginatedList<GigOverviewDto>> Handle(GetCategoryGigsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching gigs for category id: {Id}", request.CategoryId);
            
            var list = await _dbContext.Gigs
                .Include(g => g.Packages)
                .Include(g => g.Seller)
                .Where(g => g.CategoryId == request.CategoryId && !g.IsDraft)
                .AsNoTracking()
                .ProjectTo<GigOverviewDto>(_mapper.ConfigurationProvider) 
                .PaginatedListAsync(request.PageNumber, request.PageSize);

            return list;
        }
    }
}