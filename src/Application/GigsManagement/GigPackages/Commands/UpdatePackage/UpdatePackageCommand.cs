using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Gigs;
using Hive.Domain.Enums;
using MediatR;

namespace Hive.Application.GigsManagement.GigPackages.Commands.UpdatePackage
{
    public record UpdatePackageCommand(int PackageId, string Title, string Description, decimal Price, double DeliveryTime, DeliveryFrequency DeliveryFrequency,
        int? Revisions, RevisionType RevisionType) : IRequest;
    
    public class UpdatePackageCommandHandler : IRequestHandler<UpdatePackageCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdatePackageCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<Unit> Handle(UpdatePackageCommand request, CancellationToken cancellationToken)
        {
            var package = await _context.Packages.FindAsync(request.PackageId);

            if (package == null)
            {
                throw new NotFoundException(nameof(Package), request.PackageId);
            }

            _mapper.Map(request, package);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}