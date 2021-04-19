using System;
using Hive.Gig.Application.Categories.Commands;
using Hive.Gig.Application.Questions.Interfaces;
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.Features.Categories.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Core.Exceptions;
    using Domain.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    
    public record DeleteCategoryCommand(int Id) : IRequest;
    
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly ILogger<CreateCategoryCommandHandler> _logger;

        public DeleteCategoryCommandHandler(IGigManagementDbContext dbContext, ILogger<CreateCategoryCommandHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (category == null)
            {
                _logger.LogError("Category with {@Id} was not found.", request.Id);
                throw new NotFoundException(nameof(Category), request.Id);
            }

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Category with {@Id} was deleted.", request.Id);

            return Unit.Value;
        }
    }
}