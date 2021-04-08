using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Mappings;
using Hive.Domain.Entities.Gigs;
using Hive.Domain.Enums;
using MediatR;

namespace Hive.Application.GigsManagement.GigPackages.Commands.CreatePackage
{
    public class CreatePackageCommand : IRequest<int>, IMapFrom<Package>
    {
        public PackageTier PackageTier { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public decimal Price { get; set; }
        
        public double DeliveryTime { get; set; }
        
        public DeliveryFrequency DeliveryFrequency { get; set; }
        
        public int GigId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreatePackageCommand, Package>(MemberList.Source);
        }
    }

    public class CreatePackageCommandHandler : IRequestHandler<CreatePackageCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreatePackageCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<int> Handle(CreatePackageCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Package>(request);

            _context.Packages.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}