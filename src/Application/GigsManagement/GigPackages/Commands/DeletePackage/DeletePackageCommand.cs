using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Security;
using Hive.Domain.Entities.Gigs;
using MediatR;

namespace Hive.Application.GigsManagement.GigPackages.Commands.DeletePackage
{
    [Authorize(Roles = "Seller, Administrator")]
    public record DeletePackageCommand(int PackageId) : IRequest;
    
    public class DeletePackageCommandHandler : IRequestHandler<DeletePackageCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeletePackageCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(DeletePackageCommand request, CancellationToken cancellationToken)
        {
            var package = await _context.Packages.FindAsync(request.PackageId);

            if (package == null)
            {
                throw new NotFoundException(nameof(Package), request.PackageId);
            }

            _context.Packages.Remove(package);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}