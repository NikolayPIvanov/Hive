using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hive.Application.Ordering.Resolutions.Queries
{
    public record DownloadResolutionFileQuery(int ResolutionId) : IRequest<FileResponse>;

    public class DownloadResolutionFileQueryHandler : IRequestHandler<DownloadResolutionFileQuery, FileResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IFileService _fileService;

        public DownloadResolutionFileQueryHandler(IApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        
        public async Task<FileResponse> Handle(DownloadResolutionFileQuery request, CancellationToken cancellationToken)
        {
            var resolution = await _context.Resolutions.FindAsync(request.ResolutionId);
            var response = await _fileService.DownloadAsync(resolution.Location);
            return response;
        }
    }
}