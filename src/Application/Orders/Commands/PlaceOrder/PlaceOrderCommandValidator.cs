using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Orders.Commands.PlaceOrder
{
    public class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
    {
        private readonly IApplicationDbContext _context;

        public PlaceOrderCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(o => Tuple.Create(o.GigId, o.PackageId, o.OfferedById, o.TotalAmount))
                .MustAsync(DataIsValid);
        }

        private async Task<bool> DataIsValid(Tuple<int, int, int, decimal> data, CancellationToken cancellationToken)
        {
            var (gigId, packageId, sellerId, basePrice) = data;

            var gig = await _context.Gigs
                .Select(g => new
                {
                    g.Id,
                    g.SellerId,
                    Package = g.Packages.Select(p => new {p.Id,p.Price})
                        .FirstOrDefault(p => p.Id == packageId)
                        
                })
                .FirstOrDefaultAsync(g => g.Id == gigId,cancellationToken);

            if (gig is null) return false;

            var priceIsSame = gig.Package != null && gig.Package.Price == basePrice;
            var sellerIdIsSame = gig.SellerId == sellerId;

            return priceIsSame && sellerIdIsSame;
        }
    }
}