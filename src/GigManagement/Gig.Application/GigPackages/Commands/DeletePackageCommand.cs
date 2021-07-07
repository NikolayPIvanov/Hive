using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Security.Handlers;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.GigPackages.Commands
{
    public record DeletePackageCommand(int PackageId) : IRequest;
    
    public class DeletePackageCommandHandler : AuthorizationRequestHandler<Package>, IRequestHandler<DeletePackageCommand>
    {
        private readonly IGigManagementDbContext _context;
        private readonly ILogger<DeletePackageCommand> _logger;

        public DeletePackageCommandHandler(IGigManagementDbContext context, 
            ICurrentUserService currentUserService, IAuthorizationService authorizationService,
            ILogger<DeletePackageCommand> logger) : base(currentUserService, authorizationService)
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
            
            var result = await base.AuthorizeAsync(package,  new [] {"OnlyOwnerPolicy"});
            
            if (!result.All(s => s.Succeeded))
            {
                throw new ForbiddenAccessException();
            }

            _context.Packages.Remove(package);
            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Package with id: {@Id} was deleted", request.PackageId);

            return Unit.Value;
        }
    }
}