using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Gig.Application.Questions.Interfaces;
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
        public CreatePackageCommandValidator(IGigManagementDbContext dbContext)
        {
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
                .NotNull().WithMessage("A {PropertyName} must be provided")
                .GreaterThanOrEqualTo(1).WithMessage("{PropertyName} cannot be below {ComparisonValue}.");

            RuleFor(x => x.GigId)
                .MustAsync(async (id, token) => await dbContext.Gigs.AnyAsync(x => x.Id == id, token))
                .WithMessage("Must specify a valid gig.");
        }
    }

    public class CreatePackageCommandHandler : IRequestHandler<CreatePackageCommand, int>
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly ILogger<CreatePackageCommand> _logger;

        public CreatePackageCommandHandler(IGigManagementDbContext dbContext, ILogger<CreatePackageCommand> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<int> Handle(CreatePackageCommand request, CancellationToken cancellationToken)
        {
            var package = new Package(request.Title, request.Description, request.Price, request.DeliveryTime,
                request.DeliveryFrequency, request.Revisions, request.RevisionType, request.GigId);

            await _dbContext.Packages.AddAsync(package, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Package {@Id} for Gig with {@GigId} was created", package.Id, package.GigId);

            return package.Id;
        }
    }
}