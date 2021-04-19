using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Questions.Interfaces;
using Hive.Gig.Contracts.Objects;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.GigPackages.Queries
{
    public record GetPackageQuery(int Id) : IRequest<PackageDto>;

    public class GetPackageQueryHandler : IRequestHandler<GetPackageQuery, PackageDto>
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetPackageQueryHandler(IGigManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        public async Task<PackageDto> Handle(GetPackageQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Packages
                .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Package), request.Id);
            }

            return _mapper.Map<PackageDto>(entity);
        }
    }
}