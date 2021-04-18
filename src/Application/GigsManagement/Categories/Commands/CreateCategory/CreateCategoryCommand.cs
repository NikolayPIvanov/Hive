using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Security;
using Hive.Domain.Entities.Gigs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Application.GigsManagement.Categories.Commands.CreateCategory
{
    public record CreateCategoryCommand(string Title, int? ParentId = null) : IRequest<int>;
    
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        
        public CreateCategoryCommandValidator(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            
            RuleFor(c => c.Title)
                .MustAsync(BeUniqueTitleAsync).WithMessage("Category with given title already exists.")
                .MinimumLength(3).WithMessage("Title should be at minimum 3 characters")
                .MaximumLength(50).WithMessage("Title should be at maximum 50 characters")
                .NotEmpty().WithMessage("Title cannot be empty");

            RuleFor(c => c.ParentId)
                .MustAsync(ParentCategoryExistsAsync).WithMessage("Parent category should be existing one.");
        }

        private async Task<bool> ParentCategoryExistsAsync(int? parentCategoryId, CancellationToken cancellationToken)
        {
            if (parentCategoryId is null) return true;

            return await _dbContext.Categories.AnyAsync(c => c.Id == parentCategoryId, cancellationToken);
        }
        
        private async Task<bool> BeUniqueTitleAsync(string title, CancellationToken cancellationToken)
        {
            return await _dbContext.Categories.AllAsync(r => r.Title != title, cancellationToken);
        }
    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<CreateCategoryCommandHandler> _logger;

        public CreateCategoryCommandHandler(IApplicationDbContext dbContext, ILogger<CreateCategoryCommandHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var (title, parentId) = request;
            var category = new Category(title, parentId);
            
            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Category with {@Title} and {@ParentId} was created.", title, parentId);

            return category.Id;
        }
    }
}