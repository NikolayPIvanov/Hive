using System.Linq;
using AutoMapper;
using Hive.Application.Investing.Plans.Queries;
using Hive.Domain.Entities.Investing;

namespace Hive.Application.Investing.Plans
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