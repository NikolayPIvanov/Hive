using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Gigs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Application.GigsManagement.Categories.Commands.UpdateCategory
{
    public record UpdateCategoryCommand(int Id, string Title, int? ParentId = null) : IRequest;
    
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        
        public UpdateCategoryCommandValidator(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            
            RuleFor(c => c.Title)
                .MinimumLength(3).WithMessage("Title should be at minimum 3 characters")
                .MaximumLength(50).WithMessage("Title should be at maximum 50 characters")
                .NotEmpty().WithMessage("Title cannot be empty");
            
            RuleFor(c => c)
                .MustAsync(BeValidAsync).WithMessage("Either parent category does not exist, title already is taken or trying to set invalid parent id");
        }
        
        private async Task<bool> ParentCategoryExistsAsync(int parentCategoryId, CancellationToken cancellationToken)
        {
            return await _dbContext.Categories.AnyAsync(c => c.Id == parentCategoryId && c.ParentId == null, cancellationToken);
        }
        
        private async Task<bool> BeValidAsync(UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            var (id, title, parentId) = command;
            
            var titleExists = await _dbContext.Categories
                .AnyAsync(r => r.Title == title && r.Id != id, cancellationToken);

            var parentCategoryIsValid =
                !parentId.HasValue || (parentId.Value != id && await ParentCategoryExistsAsync(parentId.Value, cancellationToken));
            var titleIsValid = !(titleExists);

            return titleIsValid && parentCategoryIsValid;
        }
    }
    
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<UpdateCategoryCommandHandler> _logger;

        public UpdateCategoryCommandHandler(IApplicationDbContext dbContext, ILogger<UpdateCategoryCommandHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var (id, title, parentId) = request;
            var category = await _dbContext.Categories
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (category == null)
            {
                _logger.LogError("Category with {@Id} was not found.", request.Id);
                throw new NotFoundException(nameof(Category), id);
            }

            category.Title = title;
            category.ParentId = parentId;
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogError("Category with {@Id} was updated.", request.Id);
            
            return Unit.Value;
        }
    }
}