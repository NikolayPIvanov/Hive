using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using Hive.Common.Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;
using MimeTypes;

namespace Ordering.Application.Resolutions.Queries
{
    public record DownloadResolutionFileQuery(Guid Version) : IRequest<FileContentResult>;

    public class DownloadResolutionFileQueryHandler : IRequestHandler<DownloadResolutionFileQuery, FileContentResult>
    {
        private readonly IOrderingContext _context;
        private readonly IFileService _fileService;
        private readonly ILogger<DownloadResolutionFileQueryHandler> _logger;

        public DownloadResolutionFileQueryHandler(IOrderingContext context, IFileService fileService, ILogger<DownloadResolutionFileQueryHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<FileContentResult> Handle(DownloadResolutionFileQuery request, CancellationToken cancellationToken)
        {
            var resolution =
                await _context.Resolutions.FirstOrDefaultAsync(x => x.Version == request.Version.ToString(), cancellationToken);

            if (resolution == null)
            {
                _logger.LogWarning("Resolution with id: {@Id} was not found", request.Version);
                throw new NotFoundException(nameof(Resolution), request.Version);
            }
            
            var response = await _fileService.DownloadAsync("resolutions", resolution.Location, cancellationToken);
        
            var bytes = response.Source.ReadFully();
            
            var extension = Path.GetExtension(response.FileName).Replace(".", "");
            var mimeType = MimeTypeMap.GetMimeType(extension);
            _logger.LogInformation("File for resolution with id: {@Id} was not downloaded", request.Version);
    
            return new FileContentResult(bytes, mimeType);
        }
    }
    
    public static class StreamHelpers
    {
        public static byte[] ReadFully(this Stream input)
        {
            var ms = new MemoryStream();
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }
}