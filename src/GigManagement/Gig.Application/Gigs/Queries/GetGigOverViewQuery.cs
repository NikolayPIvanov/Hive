using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Gigs.Queries;
using Hive.Gig.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gig.Application.Gigs.Queries
{
    public record GetGigOverViewQuery(int Id) : IRequest<GigOverviewDto>;

    public class GetGigOverViewQueryHandler : IRequestHandler<GetGigOverViewQuery, GigOverviewDto>
    {
        private readonly IGigManagementDbContext _context;
        private readonly IMapper _mapper;

        public GetGigOverViewQueryHandler(IGigManagementDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<GigOverviewDto> Handle(GetGigOverViewQuery request, CancellationToken cancellationToken)
        {
            var gig = await _context.Gigs
                .AsNoTracking()
                .Include(g => g.Seller)
                .Include(g => g.Packages)
                .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

            if (gig == null)
            {
                throw new NotFoundException(nameof(Gig), request.Id);
            }

            return _mapper.Map<GigOverviewDto>(gig);
        }
    }
}