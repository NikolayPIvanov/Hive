using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using Hive.Gig.Domain.Enums;
using MediatR;

namespace Hive.Gig.Application.GigPackages.Commands
{
    public class UpdatePackageCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        
        public PackageTier PackageTier { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public decimal Price { get; set; }
        
        public double DeliveryTime { get; set; }
        
        public DeliveryFrequency DeliveryFrequency { get; set; }
        
        public int Revisions { get; set; }
    }
    
    public class UpdatePackageCommandValidator : AbstractValidator<UpdatePackageCommand>
    {
        public UpdatePackageCommandValidator(IGigManagementContext context)
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
        }
    }

    public class UpdatePackageCommandHandler : IRequestHandler<UpdatePackageCommand>
    {
        private readonly IGigManagementContext _context;

        public UpdatePackageCommandHandler(IGigManagementContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(UpdatePackageCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Packages.FindAsync(request.Id);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Package), request.Id);
            }

            // TODO: Use AutoMapper ?
            entity.Description = request.Description;
            entity.Price = request.Price;
            entity.Revisions = request.Revisions;
            entity.Title = request.Title;
            entity.DeliveryFrequency = request.DeliveryFrequency;
            entity.DeliveryTime = request.DeliveryTime;
            entity.PackageTier = request.PackageTier;

            _context.Packages.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}