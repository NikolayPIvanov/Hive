using AutoMapper;
using Hive.Investing.Application.Plans.Queries;
using Hive.Investing.Domain.Entities;

namespace Hive.Investing.Application.Plans
{
    public class PlanProfile : Profile
    {
        public PlanProfile()
        {
            CreateMap<Plan, PlanDto>()
                .ForMember(d => d.FundingNeeded, x => x.MapFrom(s => s.StartingFunds))
                .ForMember(d => d.VendorUserId, x => x.MapFrom(s => s.Vendor.UserId));
        }
    }
}