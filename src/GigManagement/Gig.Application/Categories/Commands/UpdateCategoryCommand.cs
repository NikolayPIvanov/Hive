using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Application.Exceptions;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Categories.Commands
{
    public record UpdateCategoryCommand(int Id, string Title, int? ParentId = null) : IRequest;
    
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        private readonly IGigManagementContext _context;
        
        public UpdateCategoryCommandValidator(IGigManagementContext context)
        {
            _context = context;
            
            RuleFor(c => c.Title)
                .MinimumLength(3).WithMessage("Title should be at minimum 3 characters")
                .MaximumLength(50).WithMessage("Title should be at maximum 50 characters")
                .NotEmpty().WithMessage("Title cannot be empty");
            
            RuleFor(c => c)
                .MustAsync(BeValidAsync).WithMessage("Either parent category does not exist, title already is taken or trying to set invalid parent id");
        }
        
        // TODO: Bug - Might update a category so that it has it's parent category as one of it's subcategories
        private async Task<bool> ParentCategoryExistsAsync(int parentCategoryId, CancellationToken cancellationToken)
        {
            return await _context.Categories.AnyAsync(c => c.Id == parentCategoryId, cancellationToken);
        }
        
        private async Task<bool> BeValidAsync(UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            var (id, title, parentId) = command;
            var titleExists = await _context.Categories
                .AnyAsync(r => r.Title == title && r.Id != id, cancellationToken);
            
            var titleIsValid = !titleExists;
            var parentCategoryIsValid = !parentId.HasValue || await ParentCategoryExistsAsync(parentId.Value, cancellationToken);
            
            return titleIsValid && parentCategoryIsValid;
        }
    }
    
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly IGigManagementContext _context;

        public UpdateCategoryCommandHandler(IGigManagementContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (category == null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }

            category.Title = request.Title;
            category.ParentId = request.ParentId;
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}