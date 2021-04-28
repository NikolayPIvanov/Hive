using AutoMapper;
using Billing.Application.Wallets.Queries;
using Hive.Billing.Domain.Entities;

namespace Billing.Application.Wallets
{
    public class BillingProfile : Profile
    {
        public BillingProfile()
        {
            CreateMap<Wallet, WalletDto>().DisableCtorValidation();
            CreateMap<Transaction, TransactionDto>()
                .ForMember(d => d.TransactionType,
                    s => s.MapFrom(x => x.TransactionType.ToString()));
        }
    }
}