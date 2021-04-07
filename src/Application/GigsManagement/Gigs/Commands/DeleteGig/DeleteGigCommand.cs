using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Gigs;
using MediatR;

namespace Hive.Application.Gigs.Commands.DeleteGig
{
    public record DeleteGigCommand(int Id) : IRequest;

    public class DeleteGigCommandHandler : IRequestHandler<DeleteGigCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public DeleteGigCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteGigCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Gigs.FindAsync(request.Id);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Gig), request.Id);
            }

            _dbContext.Gigs.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}