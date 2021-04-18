using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Security;
using Hive.Domain.Entities.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Application.Ordering.Resolutions.Queries
{
    public record GetResolutionByIdQuery(int ResolutionId) : IRequest<ResolutionDto>;

    public class GetResolutionByIdQueryHandler : IRequestHandler<GetResolutionByIdQuery, ResolutionDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<GetResolutionsQueryHandler> _logger;

        public GetResolutionByIdQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService,
            ILogger<GetResolutionsQueryHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _logger = logger;
        }
        
        public async Task<ResolutionDto> Handle(GetResolutionByIdQuery request, CancellationToken cancellationToken)
        {
            var resolution = await _context.Resolutions
                .Include(x => x.Order)
                .FirstOrDefaultAsync(x => x.Id == request.ResolutionId, cancellationToken);
            if (resolution == null)
            {
                throw new NotFoundException(nameof(Resolution), request.ResolutionId);
            }

            return _mapper.Map<Resolution, ResolutionDto>(resolution);
        }
    }
}