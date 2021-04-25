using AutoMapper;
using Hive.Gig.Domain.Entities;
using Hive.Gig.Domain.Enums;

namespace Hive.Gig.Application.GigPackages
{
    public class PackageProfile : Profile
    {
        public PackageProfile()
        {
            CreateMap<Package, PackageDto>()
                .ForMember(d => d.PackageTier, x => x.MapFrom(s => s.PackageTier.ToString()))
                .ForMember(d => d.DeliveryFrequency, x => x.MapFrom(s => s.DeliveryFrequency.ToString()))
                .AfterMap((package, dto) =>
                {
                    if (package.Revisions != null)
                        dto.Revisions = package.RevisionType == RevisionType.Numeric
                            ? package.Revisions.Value.ToString()
                            : RevisionType.Unlimited.ToString();
                });
        }
    }
}