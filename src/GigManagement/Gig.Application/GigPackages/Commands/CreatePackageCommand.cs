using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Core;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Security;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using Hive.Gig.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.GigPackages.Commands
{
    
    //
    // public record CreatePackageCommand(ICollection<PackageModel> Packages) : IRequest<int>;
    //
    //
    //
    // public class CreatePackageCommandHandler : IRequestHandler<CreatePackageCommand, int>
    // {
    //     private readonly IGigManagementDbContext _context;
    //     private readonly ICurrentUserService _currentUserService;
    //     private readonly ILogger<CreatePackageCommand> _logger;
    //
    //     public CreatePackageCommandHandler(IGigManagementDbContext dbContext, ICurrentUserService currentUserService,
    //         ILogger<CreatePackageCommand> logger)
    //     {
    //         _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    //         _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
    //         _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    //     }
    //     
    //     public async Task<int> Handle(CreatePackageCommand request, CancellationToken cancellationToken)
    //     {
    //         var gigId = request.Packages.First().GigId;
    //         var gig = await _context.Gigs
    //             .Include(g => g.Seller)
    //             .FirstOrDefaultAsync(x => x.Id == gigId, cancellationToken);
    //         if (gig == null)
    //         {
    //             _logger.LogWarning("Gig with id: {@Id} was not found", gigId);
    //             throw new NotFoundException(nameof(Gig), gigId);
    //         }
    //
    //         var isOwner = _currentUserService.UserId == gig.Seller.UserId;
    //         if (!isOwner)
    //         {
    //             throw new ForbiddenAccessException();
    //         }
    //
    //         var packages = request.Packages.Select(r => new Package(
    //             r.Title, r.Description, r.Price, r.DeliveryTime,
    //             r.DeliveryFrequency, r.Revisions, r.RevisionType, r.GigId));
    //
    //         await _context.Packages.AddRangeAsync(packages, cancellationToken);
    //         await _context.SaveChangesAsync(cancellationToken);
    //
    //         foreach (var package in packages)
    //         {
    //             _logger.LogInformation("Package {@Id} for Gig with {@GigId} was created", package.Id, package.GigId);
    //         }
    //
    //         // todo;
    //         return 1;
    //     }
    // }
}