using System;
using System.Threading;
using System.Threading.Tasks;
using Investor.Contracts;

namespace Investor
{
    public class InvestorRepository : IInvestorRepository
    {
        public Task<Guid> CreateInvestorAccount(Guid userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}