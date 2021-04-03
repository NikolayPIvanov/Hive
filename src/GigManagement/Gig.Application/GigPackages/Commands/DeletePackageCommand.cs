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
        private readonly IGigManagementDbContext _dbContext;

        public DeletePackageCommandHandler(IGigManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<Unit> Handle(DeletePackageCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Packages.FindAsync(request.Id);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Package), request.Id);
            }

            _dbContext.Packages.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}