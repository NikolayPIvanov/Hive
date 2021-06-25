using System.Linq;
using FluentValidation;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Gigs.Commands.CreateGig
{
    public class PackageModelValidator : AbstractValidator<PackageModel>
    {
        public PackageModelValidator(IGigManagementDbContext context)
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
}