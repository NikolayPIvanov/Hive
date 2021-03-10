using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Gigs;
using MediatR;

namespace Hive.Application.Gigs.Commands.DeleteGig
{
    public class DeleteGigCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteGigCommandHandler : IRequestHandler<DeleteGigCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteGigCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(DeleteGigCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Gigs.FindAsync(request.Id, cancellationToken);
            
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