using System;
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.Features.Categories.Commands
{
    using Interfaces;
    using Domain.Entities;

    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public record CreateCategoryCommand(string Title, int? ParentId = null) : IRequest<int>;
    
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        private readonly IGigManagementDbContext _dbContext;
        
        public CreateCategoryCommandValidator(IGigManagementDbContext dbContext)
        {
            _dbContext = dbContext;
            
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

            return (await _dbContext.Categories.AnyAsync(c => c.Id == parentCategoryId, cancellationToken));
        }
        
        private async Task<bool> UniqueTitleAsync(string title, CancellationToken cancellationToken)
        {
            var hasWithTitle = await _dbContext.Categories.AnyAsync(r => r.Title == title, cancellationToken);
            return !hasWithTitle;
        }
    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly ILogger<CreateCategoryCommandHandler> _logger;

        public CreateCategoryCommandHandler(IGigManagementDbContext dbContext, ILogger<CreateCategoryCommandHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var (title, parentId) = request;
            var category = new Category(title, parentId);
            
            // TODO: Use sequence
            await _dbContext.Categories.AddAsync(category, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Category with {@Title} and {@ParentId} was created.", title, parentId);

            return category.Id;
        }
    }
}