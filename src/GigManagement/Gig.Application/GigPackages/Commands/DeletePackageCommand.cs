using System;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Security;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.GigPackages.Commands
{
    public record DeletePackageCommand(int PackageId) : IRequest;
    
    public class DeletePackageCommandHandler : IRequestHandler<DeletePackageCommand>
    {
        private readonly IGigManagementDbContext _context;
        private readonly ILogger<DeletePackageCommand> _logger;

        public DeletePackageCommandHandler(IGigManagementDbContext context, ILogger<DeletePackageCommand> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(DeletePackageCommand request, CancellationToken cancellationToken)
        {
            var package = await _context.Packages.FindAsync(request.PackageId);

            if (package == null)
            {
                _logger.LogWarning("Package with id: {@Id} was not found", request.PackageId);
                throw new NotFoundException(nameof(Package), request.PackageId);
            }

            _context.Packages.Remove(package);
            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Package with id: {@Id} was deleted", request.PackageId);

            return Unit.Value;
        }
    }
}