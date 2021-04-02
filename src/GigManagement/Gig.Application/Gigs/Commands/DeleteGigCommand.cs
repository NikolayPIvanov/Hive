using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Interfaces;
using MediatR;

namespace Hive.Gig.Application.Gigs.Commands
{
    public record DeleteGigCommand(int Id) : IRequest;

    public class DeleteGigCommandHandler : IRequestHandler<DeleteGigCommand>
    {
        private readonly IGigManagementContext _context;

        public DeleteGigCommandHandler(IGigManagementContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteGigCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Gigs.FindAsync(request.Id);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Gig), request.Id);
            }

            _context.Gigs.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}