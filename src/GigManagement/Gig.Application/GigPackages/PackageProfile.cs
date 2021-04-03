using AutoMapper;
using Hive.Gig.Contracts.Objects;
using Hive.Gig.Domain.Entities;

namespace Hive.Gig.Application.GigPackages
{
    public class PackageProfile : Profile
    {
        public PackageProfile()
        {
            CreateMap<Package, PackageDto>()
                .ForMember(d => d.PackageTier, x => x.MapFrom(s => s.PackageTier.ToString()))
                .ForMember(d => d.DeliveryFrequency, x => x.MapFrom(s => s.DeliveryFrequency.ToString()));
        }
    }
}