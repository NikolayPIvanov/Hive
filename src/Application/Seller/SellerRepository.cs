using System;
using System.Threading;
using System.Threading.Tasks;

namespace Seller
{
    public class SellerRepository : ISellerRepository
    {
        private readonly SellerDbContext _context;

        public SellerRepository(SellerDbContext context)
        {
            _context = context;
        }
        
        public async Task<Guid> CreateSellerAccount(Guid id, CancellationToken cancellationToken = default)
        {
            Domain.Seller.Seller seller = new(id);

            _context.Sellers.Add(seller);
            await _context.SaveChangesAsync(cancellationToken);

            return seller.Id;
        }
    }
}