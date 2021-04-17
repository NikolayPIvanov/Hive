using AutoMapper;
using Hive.Domain.Entities.Billing;

namespace Hive.Application.Billing.Transactions.Queries
{
    public class TransactionsProfile : Profile
    {
        public TransactionsProfile()
        {
            CreateMap<Transaction, TransactionDto>().DisableCtorValidation();
        }
    }
}