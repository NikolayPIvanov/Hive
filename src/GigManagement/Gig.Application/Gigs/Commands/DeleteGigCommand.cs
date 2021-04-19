using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Questions.Interfaces;
using MediatR;

namespace Hive.Gig.Application.Gigs.Commands
{
    public record DeleteGigCommand(int Id) : IRequest;

    public class DeleteGigCommandHandler : IRequestHandler<DeleteGigCommand>
    {
        private readonly IGigManagementDbContext _dbContext;

        public DeleteGigCommandHandler(IGigManagementDbContext dbContext)
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