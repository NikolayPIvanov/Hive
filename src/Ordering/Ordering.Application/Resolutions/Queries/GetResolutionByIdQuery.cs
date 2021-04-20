using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.SeedWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Application.Resolutions.Queries
{
    public record ResolutionDto(string Version, string Blob, Guid OrderNumber, bool IsAccepted = false);
    public record GetResolutionByIdQuery(int ResolutionId) : IRequest<ResolutionDto>;

    public class GetResolutionByIdQueryHandler : IRequestHandler<GetResolutionByIdQuery, ResolutionDto>
    {
        private readonly IOrderingContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<GetResolutionByIdQueryHandler> _logger;

        public GetResolutionByIdQueryHandler(IOrderingContext context, IMapper mapper, ICurrentUserService currentUserService,
            ILogger<GetResolutionByIdQueryHandler> logger)
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