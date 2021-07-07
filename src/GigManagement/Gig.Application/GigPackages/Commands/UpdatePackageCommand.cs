using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Hive.Common.Core;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Security.Handlers;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using Hive.Gig.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.GigPackages.Commands
{
    public record UpdatePackageCommand(int PackageId, int GigId, PackageTier PackageTier, string Title, string Description, decimal Price, 
        double DeliveryTime, DeliveryFrequency DeliveryFrequency, int? Revisions, RevisionType RevisionType) : IRequest;
    
    public class UpdatePackageCommandValidator : AbstractValidator<UpdatePackageCommand>
    {
        public UpdatePackageCommandValidator(IGigManagementDbContext context)
        {
            RuleFor(x => new {x.PackageId, x.GigId, x.PackageTier})
                .MustAsync(async (pair, token) =>
                {
                    return await context.Packages.Where(x => x.GigId == pair.GigId && x.Id != pair.PackageId)
                        .AllAsync(p => p.PackageTier != pair.PackageTier, token);
                }).WithMessage("Package with selected tier already is present on the gig or the gig does not exist.");
            
            RuleFor(x => x.Title)
                .MaximumLength(50).WithMessage("{PropertyName} cannot have more than {MaxLength} characters.")
                .MinimumLength(3).WithMessage("{PropertyName} cannot have less than {MinLength} characters.")
                .NotEmpty().WithMessage("A {PropertyName} must be provided");
            
            RuleFor(x => x.Description)
                .MaximumLength(100).WithMessage("{PropertyName} cannot have more than {MaxLength} characters.")
                .MinimumLength(10).WithMessage("{PropertyName} cannot have less than {MinLength} characters.")
                .NotEmpty().WithMessage("A {PropertyName} must be provided");
            
            RuleFor(x => x.Price)
                .NotNull().WithMessage("A {PropertyName} must be provided")
                .GreaterThan(0.0m).WithMessage("{PropertyName} cannot be below {ComparisonValue}.");
            
            RuleFor(x => x.DeliveryTime)
                .NotNull().WithMessage("A {PropertyName} must be provided")
                .GreaterThanOrEqualTo(1.0d).WithMessage("{PropertyName} cannot be below {ComparisonValue}.");
            
            RuleFor(x => x.Revisions)
                .NotNull().WithMessage("A {PropertyName} must be provided").When(x => x.Revisions.HasValue)
                .GreaterThanOrEqualTo(1).WithMessage("{PropertyName} cannot be below {ComparisonValue}.").When(x => x.Revisions.HasValue);
        }
    }
    
    public class UpdatePackageCommandHandler : AuthorizationRequestHandler<Package>, IRequestHandler<UpdatePackageCommand>
    {
        private readonly IGigManagementDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdatePackageCommandHandler> _logger;

        public UpdatePackageCommandHandler(IGigManagementDbContext context, IMapper mapper,
            ICurrentUserService currentUserService, IAuthorizationService authorizationService,
            ILogger<UpdatePackageCommandHandler> logger) : base(currentUserService, authorizationService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(UpdatePackageCommand request, CancellationToken cancellationToken)
        {
            var package = await _context.Packages.FindAsync(request.PackageId);

            if (package == null)
            {
                _logger.LogWarning("Package with id: {@Id} was not found", request.PackageId);
                throw new NotFoundException(nameof(Package), request.PackageId);
            }
            
            var result = await base.AuthorizeAsync(package,  new [] {"OnlyOwnerPolicy"});
            
            if (!result.All(s => s.Succeeded))
            {
                throw new ForbiddenAccessException();
            }

            _mapper.Map(request, package);
            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogWarning("Package with id: {@Id} was updated", request.PackageId);

            return Unit.Value;
        }
    }
}