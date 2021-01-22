using System;
using System.Threading;
using System.Threading.Tasks;

namespace Investor.Contracts
{
    public interface IInvestorRepository
    {
        Task<Guid> CreateInvestorAccount(Guid userId, CancellationToken cancellationToken = default);
    }
}