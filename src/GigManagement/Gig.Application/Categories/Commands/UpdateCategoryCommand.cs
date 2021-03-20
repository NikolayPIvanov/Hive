using System;
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
            
            RuleFor(c => c)
                .MustAsync(UniqueTitleAsync);

            RuleFor(c => c.Title)
                .MaximumLength(50)
                .NotEmpty();

            RuleFor(c => c.ParentId)
                .MustAsync(ParentCategoryExistsAsync);
        }

        private async Task<bool> ParentCategoryExistsAsync(int? parentCategoryId, CancellationToken cancellationToken)
        {
            if (parentCategoryId is null) return true;
            return (await _context.Categories.AnyAsync(c => c.Id == parentCategoryId, cancellationToken));
        }
        
        private async Task<bool> UniqueTitleAsync(UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            var (id, title, _) = command;
            var hasWithTitle = await _context.Categories
                .AnyAsync(r => r.Title == title && r.Id != id, cancellationToken);

            var titleIsValid = !hasWithTitle;
            
            return titleIsValid;
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