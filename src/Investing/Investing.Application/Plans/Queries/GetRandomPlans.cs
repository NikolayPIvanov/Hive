using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Mappings;
using Hive.Common.Core.Models;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Investing.Application.Plans.Queries
{
    public record GetRandomPlansQuery(PaginatedQuery Query, string? Key) : IRequest<PaginatedList<PlanDto>>;

    public class GetRandomPlansQueryHandler : IRequestHandler<GetRandomPlansQuery, PaginatedList<PlanDto>>
    {
        private readonly IInvestingDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetRandomPlansQueryHandler(IInvestingDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        
        public async Task<PaginatedList<PlanDto>> Handle(GetRandomPlansQuery request, CancellationToken cancellationToken)
        {
            var query =
                _context.Plans.Include(p => p.Vendor)
                    .Include(p => p.Vendor)
                    .AsNoTracking();
            
            if (!string.IsNullOrEmpty(request.Key))
            {
                query = query.Where(x => x.Title.ToLower().Contains(request.Key));
            }
            
            var paginatedList = 
                await query.ProjectTo<PlanDto>(_mapper.ConfigurationProvider)
                    .PaginatedListAsync(request.Query.PageNumber, request.Query.PageSize);

            return paginatedList;
        }
    }
}