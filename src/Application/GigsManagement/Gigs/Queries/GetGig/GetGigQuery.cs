using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Security;
using Hive.Domain.Entities.Gigs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.GigsManagement.Gigs.Queries.GetGig
{
    [Authorize]
    public record GetGigQuery(int Id) : IRequest<GigDto>;
    
    public class GetGigQueryHandler : IRequestHandler<GetGigQuery, GigDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetGigQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        public async Task<GigDto> Handle(GetGigQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Gigs
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