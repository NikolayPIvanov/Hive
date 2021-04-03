using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Contracts.Objects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.GigScopes.Queries
{
    public record GetScopeByIdQuery(int GigId, int ScopeId) : IRequest<GigScopeDto>;

    public class GetScopeByIdQueryHandler : IRequestHandler<GetScopeByIdQuery, GigScopeDto>
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetScopeByIdQueryHandler(IGigManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        public async Task<GigScopeDto> Handle(GetScopeByIdQuery request, CancellationToken cancellationToken)
        {
            var gig = await _dbContext.Gigs
                .Select(x => new
                {
                    x.Id,
                    x.GigScope
                })
                .FirstOrDefaultAsync(g => g.Id == request.GigId && g.GigScope.Id == request.ScopeId,cancellationToken);
            
            if (gig is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Gig), request.GigId);
            }

            return _mapper.Map<GigScopeDto>(gig.GigScope);
        }
    }
}