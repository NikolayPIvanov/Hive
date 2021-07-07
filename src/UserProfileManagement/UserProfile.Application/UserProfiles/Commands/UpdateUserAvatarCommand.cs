using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using Hive.Common.Core.Exceptions;
using Hive.UserProfile.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.UserProfile.Application.UserProfiles.Commands
{
    public record UpdateUserAvatarCommand(int ProfileId, string Extension, MemoryStream FileStream) : IRequest;

    public class UpdateUserAvatarCommandHandler : IRequestHandler<UpdateUserAvatarCommand>
    {
        private readonly IUserProfileDbContext _context;
        private readonly IFileService _fileService;
        private readonly ILogger<UpdateUserAvatarCommand> _logger;

        public UpdateUserAvatarCommandHandler(IUserProfileDbContext context, IFileService fileService, ILogger<UpdateUserAvatarCommand> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<Unit> Handle(UpdateUserAvatarCommand request, CancellationToken cancellationToken)
        {
            var avatarFile = await _context.UserProfiles
                .FirstOrDefaultAsync(x => x.Id == request.ProfileId, cancellationToken);

            if (avatarFile == null)
            {
                throw new NotFoundException(nameof(UserProfile), request.ProfileId);
            }

            if (!string.IsNullOrEmpty(avatarFile.AvatarUri))
            {
                var deleted = await _fileService.DeleteAsync("user-avatars", avatarFile.AvatarUri, cancellationToken);
            }
            
            var uri = await _fileService.UploadAsync("user-avatars", request.FileStream, request.Extension, cancellationToken);
            if (uri != null)
            {
                avatarFile.AvatarUri = uri;
                await _context.SaveChangesAsync(cancellationToken);
            }
            
            return Unit.Value;
        }
    }
}