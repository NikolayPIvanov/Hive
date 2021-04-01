using System.Linq;
using AutoMapper;
using Hive.Investing.Domain.Entities;

namespace Hive.Investing.Application.Plans.Queries
{
    public class PlanProfile : Profile
    {
        public PlanProfile()
        {
            CreateMap<Plan, PlanDto>()
                .ForMember(d => d.Tags, x => x.MapFrom(s => s.Tags.Select(t => t.Value)));
        }
    }
}