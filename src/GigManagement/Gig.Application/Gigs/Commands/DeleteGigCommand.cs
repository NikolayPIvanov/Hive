using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Security.Handlers;
using Hive.Gig.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.Gigs.Commands
{
    using Domain.Entities;
    
    public record DeleteGigCommand(int Id) : IRequest;

    public class DeleteGigCommandHandler : AuthorizationRequestHandler<Gig>, IRequestHandler<DeleteGigCommand>
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly ILogger<DeleteGigCommandHandler> _logger;

        public DeleteGigCommandHandler(IGigManagementDbContext dbContext,
            ICurrentUserService currentUserService, IAuthorizationService authorizationService, ILogger<DeleteGigCommandHandler> logger) 
            : base(currentUserService, authorizationService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(DeleteGigCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Gigs.FindAsync(request.Id);

            if (entity is null)
            {
                _logger.LogWarning("Gig with id: {Id} was not found", request.Id);
                throw new NotFoundException(nameof(Gig), request.Id);
            }
            
            var result = await base.AuthorizeAsync(entity,  new [] {"OnlyOwnerPolicy"});
            
            if (!result.All(s => s.Succeeded))
            {
                throw new ForbiddenAccessException();
            }

            _dbContext.Gigs.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Gig with id: {Id} was removed", request.Id);

            return Unit.Value;
        }
    }
}