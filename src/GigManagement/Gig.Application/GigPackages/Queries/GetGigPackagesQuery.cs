using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Mappings;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Contracts.Objects;
using MediatR;

namespace Hive.Gig.Application.GigPackages.Queries
{
    public record GetGigPackagesQuery(int GigId) : IRequest<IEnumerable<PackageDto>>;

    public class GetGigPackagesQueryHandler : IRequestHandler<GetGigPackagesQuery, IEnumerable<PackageDto>>
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetGigPackagesQueryHandler(IGigManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<PackageDto>> Handle(GetGigPackagesQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Packages
                .Where(p => p.GigId == request.GigId)
                .ProjectToListAsync<PackageDto>(_mapper.ConfigurationProvider);
        }
    }
}