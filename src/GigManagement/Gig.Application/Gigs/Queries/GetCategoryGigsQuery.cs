using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Mappings;
using Hive.Common.Core.Models;
using Hive.Gig.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.Gigs.Queries
{
    public record GetGigsQuery(int PageNumber = 1, int PageSize = 10, string? SearchKey = null) : PaginatedQuery(
        PageNumber, PageSize);
    public record GetCategoryGigsQuery(int? CategoryId, GetGigsQuery Query) : IRequest<PaginatedList<GigOverviewDto>>;
    
    public class GetCategoryGigsQueryHandler : IRequestHandler<GetCategoryGigsQuery, PaginatedList<GigOverviewDto>>
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCategoryGigsQueryHandler> _logger;

        public GetCategoryGigsQueryHandler(IGigManagementDbContext dbContext, ICurrentUserService currentUserService, IMapper mapper, ILogger<GetCategoryGigsQueryHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<PaginatedList<GigOverviewDto>> Handle(GetCategoryGigsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching gigs for category id: {Id}", request.CategoryId);
            var (number, size, key) = request.Query;

            var query =
                _dbContext.Gigs
                    .AsNoTracking()
                    .Include(g => g.Packages)
                    .Include(g => g.Seller)
                    .AsQueryable();

            if (key != null)
            {
                query = query
                    .Where(g => g.Title.ToLowerInvariant().Contains(key.ToLowerInvariant()));
            }

            if (request.CategoryId.HasValue)
            {
                query = query.Where(g => g.CategoryId == request.CategoryId && !g.IsDraft);
            }
            else
            {
                query = query.Where(g => g.Seller.UserId == _currentUserService.UserId);
            }
                
            var list = 
                await query
                .ProjectTo<GigOverviewDto>(_mapper.ConfigurationProvider) 
                .PaginatedListAsync(number, size);

            return list;
        }
    }
}