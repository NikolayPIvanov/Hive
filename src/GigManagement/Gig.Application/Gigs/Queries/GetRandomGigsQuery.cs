using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Mappings;
using Hive.Gig.Application.Interfaces;
using MediatR;

namespace Hive.Gig.Application.Gigs.Queries
{
    public record GetRandomGigsQuery(int Quantity = 12) : IRequest<ICollection<GigDto>>;

    public class GetRandomGigsQueryHandler : IRequestHandler<GetRandomGigsQuery, ICollection<GigDto>>
    {
        private readonly IGigManagementDbContext _context;
        private readonly IMapper _mapper;

        public GetRandomGigsQueryHandler(IGigManagementDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<ICollection<GigDto>> Handle(GetRandomGigsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Gigs
                .OrderByDescending(g => g.Created)
                .Take(request.Quantity)
                .ProjectToListAsync<GigDto>(_mapper.ConfigurationProvider);
        }
    }
}