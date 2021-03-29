using AutoMapper;
using Billing.Domain;
using Hive.Billing.Contracts.Objects;

namespace Billing.Application.Transactions
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Transaction, TransactionDto>()
                .ForMember(d => d.TransactionType,
                    s => s.MapFrom(x => x.TransactionType.ToString()));
        }
    }
}