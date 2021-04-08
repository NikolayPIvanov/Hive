using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Mappings;
using MediatR;

namespace Hive.Application.GigsManagement.GigPackages.Queries.GetGigPackages
{
    public record GetGigPackagesQuery(int GigId) : IRequest<IEnumerable<PackageDto>>;

    public class GetGigPackagesQueryHandler : IRequestHandler<GetGigPackagesQuery, IEnumerable<PackageDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetGigPackagesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<PackageDto>> Handle(GetGigPackagesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Packages
                .Where(p => p.GigId == request.GigId)
                .ProjectToListAsync<PackageDto>(_mapper.ConfigurationProvider);
        }
    }
}