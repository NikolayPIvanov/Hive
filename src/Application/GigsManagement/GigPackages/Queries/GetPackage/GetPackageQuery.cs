using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.GigsManagement.GigPackages.Queries.GetPackage
{
    public record GetPackageQuery(int Id) : IRequest<PackageDto>;

    public class GetPackageQueryHandler : IRequestHandler<GetPackageQuery, PackageDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPackageQueryHandler(IApplicationDbContext context, IMapper mapper)
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
                throw new NotFoundException(nameof(entity), request.Id);
            }

            return _mapper.Map<PackageDto>(entity);
        }
    }
}