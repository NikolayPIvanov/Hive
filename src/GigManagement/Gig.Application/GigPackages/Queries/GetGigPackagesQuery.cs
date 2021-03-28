using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Application.Mappings;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Contracts.Objects;
using MediatR;

namespace Hive.Gig.Application.GigPackages.Queries
{
    public record GetGigPackagesQuery(int GigId) : IRequest<IEnumerable<PackageDto>>;

    public class GetGigPackagesQueryHandler : IRequestHandler<GetGigPackagesQuery, IEnumerable<PackageDto>>
    {
        private readonly IGigManagementContext _context;
        private readonly IMapper _mapper;

        public GetGigPackagesQueryHandler(IGigManagementContext context, IMapper mapper)
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