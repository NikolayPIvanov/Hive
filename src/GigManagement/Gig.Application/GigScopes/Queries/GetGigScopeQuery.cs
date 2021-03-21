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
    public record GetGigScopeQuery(int GigId) : IRequest<GigScopeDto>;

    public class GetGigScopeQueryHandler : IRequestHandler<GetGigScopeQuery, GigScopeDto>
    {
        private readonly IGigManagementContext _context;
        private readonly IMapper _mapper;

        public GetGigScopeQueryHandler(IGigManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<GigScopeDto> Handle(GetGigScopeQuery request, CancellationToken cancellationToken)
        {
            var gig = await _context.Gigs
                .Select(g => new
                {
                    g.Id, g.GigScope
                })
                .FirstOrDefaultAsync(g => g.Id == request.GigId,cancellationToken);
            
            if (gig is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Gig), request.GigId);
            }

            return _mapper.Map<GigScopeDto>(gig.GigScope);
        }
    }
}