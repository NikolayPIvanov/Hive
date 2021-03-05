using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateCategoryCommandValidator(IApplicationDbContext context)
        {
            _context = context;
            
            RuleFor(c => c.Title)
                .MustAsync(UniqueTitleAsync)
                .MaximumLength(200)
                .NotEmpty();
        }
        
        // TODO: Might become problem for bigger datasets
        private async Task<bool> UniqueTitleAsync(string title, CancellationToken cancellationToken)
        {
            var hasWithTitle = await _context.Categories.AnyAsync(r => r.Title == title, cancellationToken);
            return hasWithTitle;
        }
    }
}