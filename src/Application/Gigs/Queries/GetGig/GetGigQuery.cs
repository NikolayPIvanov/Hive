using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Gigs.Queries.GetGig
{

    public record GetGigQuery(int Id) : IRequest<GigDto>;

    public class GetGigQueryHandler : IRequestHandler<GetGigQuery, GigDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetGigQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<GigDto> Handle(GetGigQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Gigs
                .Include(g => g.Questions)
                .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(entity), request.Id);
            }

            return _mapper.Map<GigDto>(entity);
        }
    }
}