using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Gigs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Gigs.Queries.GetScopeById
{
    public record GetScopeByIdQuery(int GigId, int ScopeId) : IRequest<GigScopeDto>;

    public class GetScopeByIdQueryHandler : IRequestHandler<GetScopeByIdQuery, GigScopeDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetScopeByIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
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
                throw new NotFoundException(nameof(Gig), request.GigId);
            }

            return _mapper.Map<GigScopeDto>(gig.GigScope);
        }
    }
}