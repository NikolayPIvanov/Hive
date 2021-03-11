﻿using System.Threading;
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

            RuleFor(c => c.ParentId)
                .MustAsync(ParentCategoryExistsAsync);
        }

        private async Task<bool> ParentCategoryExistsAsync(int? parentCategoryId, CancellationToken cancellationToken)
        {
            if (parentCategoryId is null)
            {
                return true;
            }

            return (await _context.Categories.AnyAsync(c => c.Id == parentCategoryId, cancellationToken));
        }
        
        private async Task<bool> UniqueTitleAsync(string title, CancellationToken cancellationToken)
        {
            var hasWithTitle = await _context.Categories.AnyAsync(r => r.Title == title, cancellationToken);
            return !hasWithTitle;
        }
    }
}