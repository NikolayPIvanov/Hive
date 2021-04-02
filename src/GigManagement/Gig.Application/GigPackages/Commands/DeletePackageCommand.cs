using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;

namespace Hive.Gig.Application.GigPackages.Commands
{
    public record DeletePackageCommand(int Id) : IRequest;

    public class DeletePackageCommandHandler : IRequestHandler<DeletePackageCommand>
    {
        private readonly IGigManagementContext _context;

        public DeletePackageCommandHandler(IGigManagementContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(DeletePackageCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Packages.FindAsync(request.Id);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Package), request.Id);
            }

            _context.Packages.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}