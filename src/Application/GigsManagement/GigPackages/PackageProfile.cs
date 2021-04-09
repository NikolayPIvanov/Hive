using AutoMapper;
using Hive.Application.GigsManagement.GigPackages.Commands.UpdatePackage;
using Hive.Application.GigsManagement.GigPackages.Queries;
using Hive.Domain.Entities.Gigs;

namespace Hive.Application.GigsManagement.GigPackages
{
    public class PackageProfile : Profile
    {
        public PackageProfile()
        {
            CreateMap<UpdatePackageCommand, Package>()
                .ForMember(d => d.Id, x => x.Ignore());

            CreateMap<PackageDto, Package>().ReverseMap();
        }
    }
}