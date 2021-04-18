using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Mappings;
using Hive.Application.Common.Security;
using Hive.Domain.Entities.Gigs;
using Hive.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.GigsManagement.GigPackages.Commands.CreatePackage
{
    [Authorize(Roles = "Seller, Administrator")]
    public record CreatePackageCommand(string Title, string Description, decimal Price, PackageTier PackageTier,
            double DeliveryTime, DeliveryFrequency DeliveryFrequency, int? Revisions, RevisionType RevisionType, int GigId) 
        : IRequest<int>;

    public class CreatePackageCommandValidator : AbstractValidator<CreatePackageCommand>
    {
        public CreatePackageCommandValidator(IApplicationDbContext context)
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
                .NotNull().WithMessage("A {PropertyName} must be provided")
                .GreaterThan(0.0m).WithMessage("{PropertyName} cannot be below {ComparisonValue}.");
            
            RuleFor(x => x.DeliveryTime)
                .NotNull().WithMessage("A {PropertyName} must be provided")
                .GreaterThanOrEqualTo(1.0d).WithMessage("{PropertyName} cannot be below {ComparisonValue}.");

            RuleFor(x => x.Revisions)
                .NotNull()
                .When(x => x.RevisionType == RevisionType.Numeric).WithMessage("A {PropertyName} must be provided")
                .GreaterThanOrEqualTo(1).When(x => x.Revisions.HasValue)
                .WithMessage("{PropertyName} cannot be below {ComparisonValue}.");
        }
    }

    public class CreatePackageCommandHandler : IRequestHandler<CreatePackageCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreatePackageCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<int> Handle(CreatePackageCommand request, CancellationToken cancellationToken)
        {
            var gigIsValid = await _context.Gigs.AnyAsync(x => x.Id == request.GigId, cancellationToken);
            if (!gigIsValid)
            {
                throw new NotFoundException(nameof(Gig), request.GigId);
            }
            
            var package = new Package(request.Title, request.Description, request.Price, request.DeliveryTime,
                request.DeliveryFrequency, request.Revisions, request.RevisionType, request.GigId);

            _context.Packages.Add(package);
            await _context.SaveChangesAsync(cancellationToken);

            return package.Id;
        }
    }
}