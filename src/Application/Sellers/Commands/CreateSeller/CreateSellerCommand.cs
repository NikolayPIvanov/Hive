using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Accounts;
using Hive.Domain.Entities.Gigs;
using MediatR;

namespace Hive.Application.Sellers.Commands.CreateSeller
{
    public static class CreateSellerCommand
    {
        public record Command(string UserId, int UserProfileId) : IRequest<Response>;
        public record Response(int Id);

        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context;
            }
            
            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var (userId, userProfileId) = request;
                var seller = new Seller(userId);

                _context.Sellers.Add(seller);
                await _context.SaveChangesAsync(cancellationToken);

                return new Response(seller.Id);
            }
        }
    }
}