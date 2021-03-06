﻿using System;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Security;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using Hive.Identity.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.Categories.Commands
{
    public record DeleteCategoryCommand(int Id) : IRequest;
    
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly IFileService _fileService;
        private readonly ILogger<CreateCategoryCommandHandler> _logger;

        public DeleteCategoryCommandHandler(IGigManagementDbContext dbContext, IFileService fileService, ILogger<CreateCategoryCommandHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
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

            if (!string.IsNullOrEmpty(category.ImageLocation))
            {
                var operationResult = await _fileService.DeleteAsync("category-thumbnails", category.ImageLocation, cancellationToken);
            }

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Category with {@Id} was deleted.", request.Id);

            return Unit.Value;
        }
    }
}