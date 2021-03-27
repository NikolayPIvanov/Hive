using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Gig.Contracts;
using Hive.Common.Application.Exceptions;
using Hive.Gig.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Gigs.Queries
{
    public record GetGigQuery(int Id) : IRequest<GigDto>;
    
    public class GetGigQueryHandler : IRequestHandler<GetGigQuery, GigDto>
    {
        private readonly IGigManagementContext _context;
        private readonly IMapper _mapper;

        public GetGigQueryHandler(IGigManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<GigDto> Handle(GetGigQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Gigs
                .Include(g => g.GigScope)
                .Include(g => g.Tags)
                .Include(g => g.Category)
                .Include(g => g.Packages)
                .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Gig), request.Id);
            }

            return _mapper.Map<GigDto>(entity);
        }
    }
}