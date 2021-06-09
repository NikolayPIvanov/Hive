using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Gigs.Queries
{
    public record GetGigByPackageIdQuery(int PackageId) : IRequest<GigDto>;

    public class GetGigByPackageIdQueryHandler : IRequestHandler<GetGigByPackageIdQuery, GigDto>
    {
        private readonly IGigManagementDbContext _context;
        private readonly IMapper _mapper;

        public GetGigByPackageIdQueryHandler(IGigManagementDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<GigDto> Handle(GetGigByPackageIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Gigs
                .Include(g => g.Category)
                .Include(g => g.Packages)
                .Include(g => g.Seller)
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Packages.Any(p => p.Id == request.PackageId), cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            return _mapper.Map<GigDto>(entity);
        }
    }
}