using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Security;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;

namespace Hive.Gig.Application.GigPackages.Commands
{
    [Authorize(Roles = "Seller, Administrator")]
    public record DeletePackageCommand(int PackageId) : IRequest;
    
    public class DeletePackageCommandHandler : IRequestHandler<DeletePackageCommand>
    {
        private readonly IGigManagementDbContext _context;

        public DeletePackageCommandHandler(IGigManagementDbContext context)
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