using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Mappings;
using Hive.Domain.Entities.Gigs;
using MediatR;

namespace Hive.Application.Gigs.Commands.CreateGig
{
    public class CreateGigCommand : IRequest<int>, IMapFrom<Gig>
    {
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string? Metadata { get; set; }
        
        public string Tags { get; set; }

        public int CategoryId { get; set; }

        public List<QuestionDto> Questions { get; set; }
    }

    public class CreateGigCommandHandler : IRequestHandler<CreateGigCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateGigCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<int> Handle(CreateGigCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Gig>(request);

            _context.Gigs.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}