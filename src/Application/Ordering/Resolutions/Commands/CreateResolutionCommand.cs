using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Orders;
using MediatR;

namespace Hive.Application.Ordering.Resolutions.Commands
{
    public record CreateResolutionCommand(int OrderId, string Version, string Location) : IRequest<int>;

    public class CreateResolutionCommandHandler : IRequestHandler<CreateResolutionCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreateResolutionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        
        public async Task<int> Handle(CreateResolutionCommand request, CancellationToken cancellationToken)
        {
            var resolution = new Resolution(request.Version, request.Location, request.OrderId);

            _context.Resolutions.Add(resolution);
            await _context.SaveChangesAsync(cancellationToken);

            return resolution.Id;
        }
    }
}