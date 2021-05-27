using AutoMapper;
using Billing.Application.Wallets.Queries;
using Hive.Billing.Domain.Entities;
using Hive.Common.Core.Mappings;

namespace Billing.Application.Wallets
{
    public class BillingProfile : Profile
    {
        public BillingProfile()
        {
            CreateMap<Wallet, WalletDto>()
                .ForMember(d => d.AccountHolderId, x => x.MapFrom(s => s.AccountHolderId))
                .ReverseMap()
                .DisableCtorValidation();
            
            CreateMap<Transaction, TransactionDto>()
                .ForMember(d => d.TransactionType,
                    s => s.MapFrom(x => x.TransactionType.ToString()))
                .ReverseMap()
                .DisableCtorValidation();
        }
    }
}