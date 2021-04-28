using System;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using Hive.Common.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Application.Resolutions.Queries
{
    public record DownloadResolutionFileQuery(int ResolutionId) : IRequest<FileResponse>;

    public class DownloadResolutionFileQueryHandler : IRequestHandler<DownloadResolutionFileQuery, FileResponse>
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
        
        public async Task<FileResponse> Handle(DownloadResolutionFileQuery request, CancellationToken cancellationToken)
        {
            var resolution = await _context.Resolutions.FindAsync(request.ResolutionId);

            if (resolution == null)
            {
                _logger.LogWarning("Resolution with id: {@Id} was not found", request.ResolutionId);
                throw new NotFoundException(nameof(Resolution), request.ResolutionId);
            }
            
            var response = await _fileService.DownloadAsync("resolutions", resolution.Location, cancellationToken);

            _logger.LogInformation("File for resolution with id: {@Id} was not downloaded", request.ResolutionId);

            return response;
        }
    }
}