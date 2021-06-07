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
    public record GetAvatarByUserIdQuery(string Id) : IRequest<FileContentResult >;

    public class GetAvatarByUserIdQueryHandler : IRequestHandler<GetAvatarByUserIdQuery, FileContentResult>
    {
        private readonly IUserProfileDbContext _context;
        private readonly IFileService _fileService;

        public GetAvatarByUserIdQueryHandler(IUserProfileDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        
        public async Task<FileContentResult> Handle(GetAvatarByUserIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == request.Id, cancellationToken);

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
    
}