using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Gig.Contracts;
using Hive.Common.Application.Exceptions;
using Hive.Gig.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.GigScopes.Queries
{
    public record GetScopeByIdQuery(int GigId, int ScopeId) : IRequest<GigScopeDto>;

    public class GetScopeByIdQueryHandler : IRequestHandler<GetScopeByIdQuery, GigScopeDto>
    {
        private readonly IGigManagementContext _context;
        private readonly IMapper _mapper;

        public GetScopeByIdQueryHandler(IGigManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<GigScopeDto> Handle(GetScopeByIdQuery request, CancellationToken cancellationToken)
        {
            var gig = await _context.Gigs
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