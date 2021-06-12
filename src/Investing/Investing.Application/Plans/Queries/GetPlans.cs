using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Mappings;
using Hive.Common.Core.Models;
using Hive.Investing.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Investing.Application.Plans.Queries
{
    public record GetPlansQuery(int PageNumber, int PageSize, string Key) : IRequest<PaginatedList<PlanDto>>;

    public class GetPlansQueryHandler : IRequestHandler<GetPlansQuery, PaginatedList<PlanDto>>
    {
        private readonly IInvestingDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetPlansQueryHandler(IInvestingDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        
        public async Task<PaginatedList<PlanDto>> Handle(GetPlansQuery request, CancellationToken cancellationToken)
        {
            var query = 
                _context.Plans
                    .AsNoTracking()
                    .Include(p => p.Vendor)
                    .Where(x => x.Vendor.UserId == _currentUserService.UserId);
            
            if (!string.IsNullOrEmpty(request.Key))
            {
                query = query.Where(x => x.Title.ToLower().Contains(request.Key));
            }
            
            var paginatedList = 
                await query.ProjectTo<PlanDto>(_mapper.ConfigurationProvider)
                    .PaginatedListAsync(request.PageNumber, request.PageSize);

            return paginatedList;
        }
    }
}