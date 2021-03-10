using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Mappings;
using Hive.Domain.Entities.Gigs;
using MediatR;

namespace Hive.Application.Gigs.Commands.UpdateGig
{
    public class UpdateGigCommand : IRequest, IMapFrom<Gig>
    {
        public int Id { get; set; }

        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string Tags { get; set; }
        
        public int CategoryId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateGigCommand, Gig>()
                .ForMember(d => d.Questions, x => x.Ignore())
                .ForMember(d => d.SellerId, x => x.Ignore());
        }
    }

    public class UpdateGigCommandHandler : IRequestHandler<UpdateGigCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateGigCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<Unit> Handle(UpdateGigCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Gigs.FindAsync(request.Id, cancellationToken);
            if (entity is null)
            {
                throw new NotFoundException(nameof(Gig), request.Id);
            }

            _mapper.Map(request, entity);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}