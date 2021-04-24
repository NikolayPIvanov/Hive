using System;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Security;
using Hive.Gig.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.Gigs.Commands
{
    public record DeleteGigCommand(int Id) : IRequest;

    public class DeleteGigCommandHandler : IRequestHandler<DeleteGigCommand>
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly ILogger<DeleteGigCommandHandler> _logger;

        public DeleteGigCommandHandler(IGigManagementDbContext dbContext, ILogger<DeleteGigCommandHandler> logger)
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

            _dbContext.Gigs.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Gig with id: {Id} was removed", request.Id);

            return Unit.Value;
        }
    }
}