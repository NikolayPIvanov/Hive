using AutoMapper;
using Ordering.Application.Resolutions.Queries;
using Ordering.Domain.Entities;

namespace Ordering.Application.Resolutions
{
    public class ResolutionProfile : Profile
    {
        public ResolutionProfile()
        {
            CreateMap<Resolution, ResolutionDto>().DisableCtorValidation()
                .ForMember(d => d.OrderNumber, x => x.MapFrom(s => s.Order.OrderNumber));
        }
    }
}