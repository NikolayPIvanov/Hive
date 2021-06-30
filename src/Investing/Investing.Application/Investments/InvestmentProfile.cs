using AutoMapper;
using Hive.Investing.Application.Investments.Queries;
using Hive.Investing.Domain.Entities;

namespace Hive.Investing.Application.Investments
{
    public class InvestmentProfile : Profile
    {
        public InvestmentProfile()
        {
            CreateMap<Investment, InvestmentDto>()
                .ForMember(d => d.InvestorUserId, 
                    x => x.MapFrom(s => s.Investor.UserId));
        }
    }
}