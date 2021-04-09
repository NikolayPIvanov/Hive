using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Gigs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.GigsManagement.Gigs.Commands.UpdateDraft
{
    public record UpdateDraftCommand(int GigId) : IRequest;

    public class UpdateDraftCommandHandler : IRequestHandler<UpdateDraftCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateDraftCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(UpdateDraftCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Gigs.FirstOrDefaultAsync(g => g.Id == request.GigId, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Gig), request.GigId);
            }

            entity.IsDraft = false;
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}