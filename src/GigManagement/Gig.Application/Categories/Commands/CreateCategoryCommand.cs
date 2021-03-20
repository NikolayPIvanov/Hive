using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Categories.Commands
{
    public class CreateCategoryCommand : IRequest<int>
    {
        public string Title { get; set; }

        public int? ParentId { get; set; }
    }
    
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        private readonly IGigManagementContext _context;
        
        public CreateCategoryCommandValidator(IGigManagementContext context)
        {
            _context = context;
            
            RuleFor(c => c.Title)
                .MustAsync(UniqueTitleAsync)
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
        
        private async Task<bool> UniqueTitleAsync(string title, CancellationToken cancellationToken)
        {
            var hasWithTitle = await _context.Categories.AnyAsync(r => r.Title == title, cancellationToken);
            return !hasWithTitle;
        }
    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        private readonly IGigManagementContext _context;

        public CreateCategoryCommandHandler(IGigManagementContext context)
        {
            _context = context;
        }
        
        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category(request.Title, request.ParentId);
            _context.Categories.Add(category);
            await _context.SaveChangesAsync(cancellationToken);

            return category.Id;
        }
    }
}