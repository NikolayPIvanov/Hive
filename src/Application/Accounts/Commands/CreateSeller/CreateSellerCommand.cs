﻿using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Accounts;
using MediatR;

namespace Hive.Application.Accounts.Commands.CreateSeller
{
    public static class CreateSellerCommand
    {
        public record Command(string UserId) : IRequest<Response>;
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
                var profile = new UserProfile();
                var seller = new Seller { UserId = request.UserId, UserProfile = profile};

                _context.Sellers.Add(seller);
                await _context.SaveChangesAsync(cancellationToken);

                return new Response(seller.Id);
            }
        }
    }
}