using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hive.Seller.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Seller.Contracts;
using Seller.Contracts.Models;

namespace Hive.Seller
{
    public class SellerRepository : ISellerRepository
    {
        private readonly ILogger<SellerRepository> _logger;
        private readonly SellerDbContext _context;

        public SellerRepository(ILogger<SellerRepository> logger, SellerDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        public async Task<Guid> CreateAccountAsync(Guid id, CancellationToken cancellationToken = default)
        {
            Domain.Seller seller = new(id);

            _context.Sellers.Add(seller);
            await _context.SaveChangesAsync(cancellationToken);

            return seller.Id;
        }

        public async Task<IEnumerable<GigDto>> GetGigsAsync()
        {
            return _context.Gigs.Select(g =>
                new GigDto()
                {
                    Id = g.Id,
                    Tags = new List<string>(),
                    Title = g.GigOverview.Title
                }).AsEnumerable();
        }

        public async Task<GigDto> GetGigAsync(Guid id)
        {
            var gig = await _context.Gigs
                .Include(g => g.GigOverview)
                .FirstOrDefaultAsync(g => g.Id == id);

            return new GigDto()
            {
                Id = gig.Id,
                Tags = gig.GigOverview.Tags.Split(",").ToList(),
                Title = gig.GigOverview.Title
            };
        }

        public async Task<Guid> CreateGigAsync(string title, List<string> tags)
        {
            var mockSellerId = await _context.Sellers.FirstAsync();
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category() {Title = "Sample"});
                await _context.SaveChangesAsync();
            }

            var category = await _context.Categories.FirstAsync();

            Gig gig = new(mockSellerId.Id) {GigOverview = new (title, category.Id, tags)};

            _context.Gigs.Add(gig);
            await _context.SaveChangesAsync();

            return gig.Id;
        }

        public async Task<SellerDto> GetAccountAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var seller = await _context.Sellers.FindAsync(id, cancellationToken);
            
            return new SellerDto()
            {
                Id = seller.Id
            };
        }

    }
}