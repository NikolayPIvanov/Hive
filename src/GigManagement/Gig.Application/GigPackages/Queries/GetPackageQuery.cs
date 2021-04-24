using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.GigPackages.Queries
{
    public record GetPackageQuery(int Id) : IRequest<PackageDto>;

    public class GetPackageQueryHandler : IRequestHandler<GetPackageQuery, PackageDto>
    {
        private readonly IGigManagementDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetGigPackagesQueryHandler> _logger;

        public GetPackageQueryHandler(IGigManagementDbContext context, IMapper mapper, ILogger<GetGigPackagesQueryHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<PackageDto> Handle(GetPackageQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Packages
                .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                _logger.LogWarning("Package with id: {@Id} was not found", request.Id);
                throw new NotFoundException(nameof(Package), request.Id);
            }

            return _mapper.Map<PackageDto>(entity);
        }
    }
}