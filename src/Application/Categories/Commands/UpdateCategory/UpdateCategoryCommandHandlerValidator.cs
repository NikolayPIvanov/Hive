using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Application.Categories.Queries.GetCategory;
using Hive.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandlerValidator : AbstractValidator<UpdateCategoryCommand>
    {
        private readonly IApplicationDbContext _context;
        
        public UpdateCategoryCommandHandlerValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(c => c)
                .MustAsync(UniqueTitleAsync);

            RuleFor(c => c.ParentCategoryId)
                .MustAsync(ParentCategoryExistsAsync)
                .NotEqual(c => c.Id);

            RuleFor(c => c.Title)
                .MaximumLength(50)
                .NotEmpty();
        }

        private async Task<bool> ParentCategoryExistsAsync(int? parentCategoryId, CancellationToken cancellationToken)
        {
            if (parentCategoryId is null)
            {
                return true;
            }

            return (await _context.Categories.AnyAsync(c => c.Id == parentCategoryId, cancellationToken));
        }
        
        private async Task<bool> UniqueTitleAsync(UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            var hasWithTitle = await _context.Categories
                .AnyAsync(r => r.Title == command.Title && r.Id != command.Id, cancellationToken);

            var titleIsValid = !hasWithTitle;
            
            return titleIsValid;
        }
    }
}