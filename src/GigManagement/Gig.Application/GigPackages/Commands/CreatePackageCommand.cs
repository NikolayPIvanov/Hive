using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using Hive.Gig.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.GigPackages.Commands
{
    public class CreatePackageCommand : IRequest<int>
    {
        public PackageTier PackageTier { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public decimal Price { get; set; }
        
        public double DeliveryTime { get; set; }
        
        public DeliveryFrequency DeliveryFrequency { get; set; }
        
        public int Revisions { get; set; }
        
        public int GigId { get; set; }
    }

    public class CreatePackageCommandValidator : AbstractValidator<CreatePackageCommand>
    {
        public CreatePackageCommandValidator(IGigManagementContext context)
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
                .MustAsync(async (id, token) => await context.Gigs.AnyAsync(x => x.Id == id, token))
                .WithMessage("Must specify a valid gig.");
        }
    }

    public class CreatePackageCommandHandler : IRequestHandler<CreatePackageCommand, int>
    {
        private readonly IGigManagementContext _context;

        public CreatePackageCommandHandler(IGigManagementContext context)
        {
            _context = context;
        }
        
        public async Task<int> Handle(CreatePackageCommand request, CancellationToken cancellationToken)
        {
            var package = new Package()
            {
                Description = request.Description,
                Price = request.Price,
                Revisions = request.Revisions,
                Title = request.Title,
                DeliveryFrequency = request.DeliveryFrequency,
                DeliveryTime = request.DeliveryTime,
                PackageTier = request.PackageTier,
                GigId = request.GigId
            };

            _context.Packages.Add(package);
            await _context.SaveChangesAsync(cancellationToken);

            return package.Id;
        }
    }
}