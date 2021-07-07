using System;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Gigs.Queries;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Categories.Queries
{
    public record GetCategoryImageQuery(int CategoryId) : IRequest<FileContentResult>;

    public class GetCategoryImageQueryHandler : IRequestHandler<GetCategoryImageQuery, FileContentResult>
    {
        private readonly IGigManagementDbContext _context;
        private readonly IFileService _fileService;

        public GetCategoryImageQueryHandler(IGigManagementDbContext context, IFileService fileService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        public async Task<FileContentResult> Handle(GetCategoryImageQuery request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken);

            if (category == null || string.IsNullOrEmpty(category.ImageLocation))
            {
                throw new NotFoundException(nameof(Category), request.CategoryId);
            }

            var file = await _fileService.DownloadAsync("category-thumbnails", category.ImageLocation, cancellationToken);
            if (file == null) throw new ArgumentNullException(nameof(file));

            var bytes = file.Source.ReadFully();
            return new FileContentResult(bytes, file.ContentType)
            {
                FileDownloadName = file.FileName
            };
        }
    }
}