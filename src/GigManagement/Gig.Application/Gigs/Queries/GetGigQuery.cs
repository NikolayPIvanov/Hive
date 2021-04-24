using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.Gigs.Queries
{
    public record GetGigQuery(int Id) : IRequest<GigDto>;
    
    public class GetGigQueryHandler : IRequestHandler<GetGigQuery, GigDto>
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GetGigQueryHandler> _logger;

        public GetGigQueryHandler(IGigManagementDbContext dbContext, IMapper mapper, ILogger<GetGigQueryHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<GigDto> Handle(GetGigQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Gigs
                .Include(g => g.Category)
                .Include(g => g.Packages)
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                _logger.LogWarning("Gig with id: {Id} was not found", request.Id);
                throw new NotFoundException(nameof(Gig), request.Id);
            }

            return _mapper.Map<GigDto>(entity);
        }
    }
}