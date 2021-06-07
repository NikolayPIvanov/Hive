using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using Hive.Common.Core.Exceptions;
using Hive.UserProfile.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hive.UserProfile.Application.UserProfiles.Queries
{
    public record GetAvatarQuery(int Id) : IRequest<FileContentResult >;

    public class GetAvatarQueryHander : IRequestHandler<GetAvatarQuery, FileContentResult>
    {
        private readonly IUserProfileDbContext _context;
        private readonly IFileService _fileService;

        public GetAvatarQueryHander(IUserProfileDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        
        public async Task<FileContentResult> Handle(GetAvatarQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.UserProfiles.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(nameof(UserProfile), request.Id);
            }

            if (string.IsNullOrEmpty(user.AvatarFile))
            {
                throw new NotFoundException("Avatar has not been found.");
            }

            var file = await _fileService.DownloadAsync("avatars", user.AvatarFile, cancellationToken);

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