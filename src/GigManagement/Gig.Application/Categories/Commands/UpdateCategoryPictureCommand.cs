using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Interfaces;
using MediatR;

namespace Hive.Gig.Application.Categories.Commands
{
    public record UpdateCategoryPictureCommand(int CategoryId, string Extension, MemoryStream FileStream) : IRequest;

    public class UpdateCategoryPictureCommandValidator : AbstractValidator<UpdateCategoryPictureCommand>
    {
        public UpdateCategoryPictureCommandValidator()
        {
            RuleFor(c => c.CategoryId).NotEmpty().WithMessage("{PropertyName} cannot be empty or default");
        }
    }

    public class UpdateCategoryPictureCommandHandler : IRequestHandler<UpdateCategoryPictureCommand>
    {
        private readonly IFileService _fileService;
        private readonly IGigManagementDbContext _context;

        public UpdateCategoryPictureCommandHandler(IFileService fileService, IGigManagementDbContext context)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        public async Task<Unit> Handle(UpdateCategoryPictureCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FindAsync(request.CategoryId);

            if (category == null)
            {
                throw new NotFoundException(nameof(category), request.CategoryId);
            }
            var thumbnailLocation = await _fileService.UploadAsync("category-thumbnails", request.FileStream, request.Extension, cancellationToken);

            if (thumbnailLocation != null)
            {
                category.ImageLocation = thumbnailLocation;
                await _context.SaveChangesAsync(cancellationToken);
            }
            return Unit.Value;
        }
    }
}