using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Gigs.Commands
{
    public record SetGigImageCommand(int Id, string Extension, MemoryStream FileStream) : IRequest;
    
    public class SetGigImageCommandHandler : IRequestHandler<SetGigImageCommand>
    {
        private readonly IGigManagementDbContext _context;
        private readonly IFileService _fileService;

        public SetGigImageCommandHandler(IGigManagementDbContext context, IFileService fileService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }
        
        public async Task<Unit> Handle(SetGigImageCommand request, CancellationToken cancellationToken)
        {
            var gig = await _context.Gigs.FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
            if (gig == null)
            {
                throw new NotFoundException(nameof(Gig), request.Id);
            }

            // Clear previous images
            foreach (var imagePath in gig.Images)
            {
                await _fileService.DeleteAsync("gig-images", imagePath.Path, cancellationToken);
            }
            
            gig.Images.Clear();

            var result = await _fileService.UploadAsync("gig-images", request.FileStream, request.Extension,
                cancellationToken);

            if (string.IsNullOrEmpty(result))
            {
                throw new Exception();
            }
            
            var path = new ImagePath(result);
            gig.Images.Add(path);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}