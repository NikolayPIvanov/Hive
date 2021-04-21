using AutoMapper;
using Billing.Application.Transactions.Queries;
using Hive.Billing.Domain.Entities;

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