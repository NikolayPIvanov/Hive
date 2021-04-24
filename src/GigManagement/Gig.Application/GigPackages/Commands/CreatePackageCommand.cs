using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Security;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using Hive.Gig.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.GigPackages.Commands
{
    public record CreatePackageCommand(string Title, string Description, decimal Price, PackageTier PackageTier,
        double DeliveryTime, DeliveryFrequency DeliveryFrequency, int? Revisions, RevisionType RevisionType, int GigId) 
        : IRequest<int>;

    public class CreatePackageCommandValidator : AbstractValidator<CreatePackageCommand>
    {
        public CreatePackageCommandValidator(IGigManagementDbContext context)
        {
            RuleFor(x => x)
                .MustAsync(async (command, token) =>
                {
                    var uniqueNameInGig = await context.Packages.Where(x => x.GigId == command.GigId)
                        .AllAsync(p => p.Title != command.Title, token);
                    
                    var uniqueTier = await context.Packages.Where(x => x.GigId == command.GigId)
                        .AllAsync(p => p.PackageTier != command.PackageTier, token);

                    return uniqueNameInGig && uniqueTier;
                }).WithMessage("Package with selected tier already is present on the gig or the gig does not exist.");
            
            
            RuleFor(x => x.Title)
                .MaximumLength(50).WithMessage("{PropertyName} cannot have more than {MaxLength} characters.")
                .MinimumLength(3).WithMessage("{PropertyName} cannot have less than {MinLength} characters.")
                .NotEmpty().WithMessage("A {PropertyName} must be provided");
            
            RuleFor(x => x.Description)
                .MaximumLength(200).WithMessage("{PropertyName} cannot have more than {MaxLength} characters.")
                .MinimumLength(10).WithMessage("{PropertyName} cannot have less than {MinLength} characters.")
                .NotEmpty().WithMessage("A {PropertyName} must be provided");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("A {PropertyName} must be provided")
                .GreaterThan(0.0m).WithMessage("{PropertyName} cannot be below {ComparisonValue}.");
            
            RuleFor(x => x.DeliveryTime)
                .NotNull().WithMessage("A {PropertyName} must be provided")
                .GreaterThanOrEqualTo(1.0d).WithMessage("{PropertyName} cannot be below {ComparisonValue}.");
            
            RuleFor(x => x.Revisions)
                .NotNull().WithMessage("A {PropertyName} must be provided")
                .GreaterThanOrEqualTo(1).WithMessage("{PropertyName} cannot be below {ComparisonValue}.");
            
            RuleFor(x => x.Revisions)
                .NotNull()
                .When(x => x.RevisionType == RevisionType.Numeric).WithMessage("A {PropertyName} must be provided")
                .GreaterThanOrEqualTo(1).When(x => x.Revisions.HasValue)
                .WithMessage("{PropertyName} cannot be below {ComparisonValue}.");
        }
    }

    public class CreatePackageCommandHandler : IRequestHandler<CreatePackageCommand, int>
    {
        private readonly IGigManagementDbContext _context;
        private readonly ILogger<CreatePackageCommand> _logger;

        public CreatePackageCommandHandler(IGigManagementDbContext dbContext, ILogger<CreatePackageCommand> logger)
        {
            _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<int> Handle(CreatePackageCommand request, CancellationToken cancellationToken)
        {
            var gigIsValid = await _context.Gigs.AnyAsync(x => x.Id == request.GigId, cancellationToken);
            if (!gigIsValid)
            {
                _logger.LogWarning("Gig with id: {Id} was not found", request.GigId);
                throw new NotFoundException(nameof(Gig), request.GigId);
            }
            
            var package = new Package(request.Title, request.Description, request.Price, request.DeliveryTime,
                request.DeliveryFrequency, request.Revisions, request.RevisionType, request.GigId);

            await _context.Packages.AddAsync(package, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Package {@Id} for Gig with {@GigId} was created", package.Id, package.GigId);

            return package.Id;
        }
    }
}