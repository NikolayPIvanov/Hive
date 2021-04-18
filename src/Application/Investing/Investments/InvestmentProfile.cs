using AutoMapper;
using Hive.Application.Investing.Investments.Queries;
using Hive.Domain.Entities.Investing;

namespace Hive.Application.Investing.Investments
{
    public class InvestmentProfile : Profile
    {
        public InvestmentProfile()
        {
            CreateMap<Investment, InvestmentDto>();
        }
    }
}