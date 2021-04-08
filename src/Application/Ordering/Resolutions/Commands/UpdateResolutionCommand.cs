using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Orders;
using MediatR;

namespace Hive.Application.Ordering.Resolutions.Commands
{
    public record UpdateResolutionCommand(int Id, string Version, string Location, bool IsApproved = false) : IRequest;
    
    public class UpdateResolutionCommandHandler : IRequestHandler<UpdateResolutionCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateResolutionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(UpdateResolutionCommand request, CancellationToken cancellationToken)
        {
            var resolution = await _context.Resolutions.FindAsync(request.Id);

            if (resolution == null)
            {
                throw new NotFoundException(nameof(Resolution), request.Id);
            }

            resolution.Location = request.Location;
            resolution.Version = request.Version;
            resolution.IsApproved = request.IsApproved;

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}