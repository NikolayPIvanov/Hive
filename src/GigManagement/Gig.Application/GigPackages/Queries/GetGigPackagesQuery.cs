using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Mappings;
using Hive.Gig.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.GigPackages.Queries
{
    public record GetGigPackagesQuery(int GigId) : IRequest<IEnumerable<PackageDto>>;

    public class GetGigPackagesQueryHandler : IRequestHandler<GetGigPackagesQuery, IEnumerable<PackageDto>>
    {
        private readonly IGigManagementDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetGigPackagesQueryHandler> _logger;

        public GetGigPackagesQueryHandler(IGigManagementDbContext context, IMapper mapper, ILogger<GetGigPackagesQueryHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<IEnumerable<PackageDto>> Handle(GetGigPackagesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching packages for gig with id: {@GigId}", request.GigId);
            
            return await _context.Packages
                .AsNoTracking()
                .Where(p => p.GigId == request.GigId)
                .ProjectToListAsync<PackageDto>(_mapper.ConfigurationProvider);
        }
    }
}