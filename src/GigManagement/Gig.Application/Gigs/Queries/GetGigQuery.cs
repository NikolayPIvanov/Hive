using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Questions.Interfaces;
using Hive.Gig.Contracts.Objects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Gigs.Queries
{
    public record GetGigQuery(int Id) : IRequest<GigDto>;
    
    public class GetGigQueryHandler : IRequestHandler<GetGigQuery, GigDto>
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetGigQueryHandler(IGigManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        public async Task<GigDto> Handle(GetGigQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Gigs
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