﻿using System;
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
    public record GetAvatarByUserIdQuery(string Id) : IRequest<FileContentResult>;

    public class GetAvatarByUserIdQueryHandler : IRequestHandler<GetAvatarByUserIdQuery, FileContentResult>
    {
        private readonly IUserProfileDbContext _context;
        private readonly IFileService _fileService;

        public GetAvatarByUserIdQueryHandler(IUserProfileDbContext context, IFileService fileService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }
        
        public async Task<FileContentResult> Handle(GetAvatarByUserIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == request.Id, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(nameof(UserProfile), request.Id);
            }
            
            if (string.IsNullOrEmpty(user.AvatarUri))
            {
                throw new NotFoundException("Avatar has not been found.");
            }

            var (stream, contentType, fileName) = 
                await _fileService.DownloadAsync("user-avatars", user.AvatarUri, cancellationToken);

            var bytes = stream.ReadFully();
            return new FileContentResult(bytes, contentType)
            {
                FileDownloadName = fileName
            };
        }
    }
    
}