using System;
using System.Threading;
using System.Threading.Tasks;

namespace Seller
{
    public interface ISellerRepository
    {
        Task<Guid> CreateSellerAccount(Guid id, CancellationToken cancellationToken = default);
    }
}