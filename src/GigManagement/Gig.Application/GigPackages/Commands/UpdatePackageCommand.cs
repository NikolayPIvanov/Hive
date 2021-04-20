using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Security;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using Hive.Gig.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.GigPackages.Commands
{
    [Authorize(Roles = "Seller, Administrator")]
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
    
    public class UpdatePackageCommandHandler : IRequestHandler<UpdatePackageCommand>
    {
        private readonly IGigManagementDbContext _context;
        private readonly IMapper _mapper;

        public UpdatePackageCommandHandler(IGigManagementDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<Unit> Handle(UpdatePackageCommand request, CancellationToken cancellationToken)
        {
            var package = await _context.Packages.FindAsync(request.PackageId);

            if (package == null)
            {
                throw new NotFoundException(nameof(Package), request.PackageId);
            }

            _mapper.Map(request, package);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}