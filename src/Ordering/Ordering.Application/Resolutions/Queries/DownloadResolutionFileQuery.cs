using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Exceptions;
using MediatR;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Application.Resolutions.Queries
{
    public record DownloadResolutionFileQuery(int ResolutionId) : IRequest<FileResponse>;

    public class DownloadResolutionFileQueryHandler : IRequestHandler<DownloadResolutionFileQuery, FileResponse>
    {
        private readonly IOrderingContext _context;
        private readonly IFileService _fileService;

        public DownloadResolutionFileQueryHandler(IOrderingContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        
        public async Task<FileResponse> Handle(DownloadResolutionFileQuery request, CancellationToken cancellationToken)
        {
            var resolution = await _context.Resolutions.FindAsync(request.ResolutionId);

            if (resolution == null)
            {
                throw new NotFoundException(nameof(Resolution), request.ResolutionId);
            }
            
            var response = await _fileService.DownloadAsync(resolution.Location);
            return response;
        }
    }
}