using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Application.Exceptions;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Contracts.Objects;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.GigPackages.Queries
{
    public record GetPackageQuery(int Id) : IRequest<PackageDto>;

    public class GetPackageQueryHandler : IRequestHandler<GetPackageQuery, PackageDto>
    {
        private readonly IGigManagementContext _context;
        private readonly IMapper _mapper;

        public GetPackageQueryHandler(IGigManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<PackageDto> Handle(GetPackageQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Packages
                .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Package), request.Id);
            }

            return _mapper.Map<PackageDto>(entity);
        }
    }
}