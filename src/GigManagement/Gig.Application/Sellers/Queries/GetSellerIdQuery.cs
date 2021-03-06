﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Security;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.Sellers.Queries
{
    public record GetSellerIdQuery : IRequest<int>;

    public class GetSellerIdQueryHandler : IRequestHandler<GetSellerIdQuery, int>
    {
        private readonly IGigManagementDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<GetSellerIdQueryHandler> _logger;

        public GetSellerIdQueryHandler(IGigManagementDbContext context, ICurrentUserService currentUserService, ILogger<GetSellerIdQueryHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<int> Handle(GetSellerIdQuery request, CancellationToken cancellationToken)
        {
            var sellerId = await _context.Sellers.Select(s => new {s.UserId, s.Id})
                .FirstOrDefaultAsync(x => x.UserId == _currentUserService.UserId, cancellationToken);

            if (sellerId == null)
            {
                _logger.LogWarning("Seller with user id: {@Id} was not found", _currentUserService.UserId);
                throw new NotFoundException(nameof(Seller));
            }
            
            return sellerId.Id;
        }
    }
}