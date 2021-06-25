using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Gigs.Queries
{
    public record GetGigImageQuery(int Id) : IRequest<FileContentResult>;
    
    public class GetGigImageQueryHandler : IRequestHandler<GetGigImageQuery, FileContentResult>
    {
        private readonly IGigManagementDbContext _context;
        private readonly IFileService _fileService;

        public GetGigImageQueryHandler(IGigManagementDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        
        public async Task<FileContentResult> Handle(GetGigImageQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Gigs.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(nameof(Gig), request.Id);
            }

            if (!user.Images.Any())
            {
                throw new NotFoundException("Avatar has not been found.");
            }

            var path = user.Images.First().Path;

            var file = await _fileService.DownloadAsync("gig_images", path, cancellationToken);

            var bytes = file.Source.ReadFully();
            return new FileContentResult(bytes, file.ContentType)
            {
                FileDownloadName = file.FileName
            };
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