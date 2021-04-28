using AutoMapper;
using Hive.Investing.Application.Plans.Queries;
using Hive.Investing.Domain.Entities;

namespace Hive.Investing.Application.Plans
{
    public class PlanProfile : Profile
    {
        public PlanProfile()
        {
            CreateMap<Plan, PlanDto>();
        }
    }
}