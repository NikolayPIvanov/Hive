using System;
using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Security;
using Hive.Domain.Entities.Orders;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Ordering.Resolutions.Commands
{
    public record UpdateResolutionCommand(int Id, string Version, IFormFile File, bool IsApproved = false) : IRequest;
    
    public class UpdateResolutionCommandHandler : IRequestHandler<UpdateResolutionCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IFileService _fileService;

        public UpdateResolutionCommandHandler(IApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        
        public async Task<Unit> Handle(UpdateResolutionCommand request, CancellationToken cancellationToken)
        {
            var resolution = await _context.Resolutions.FindAsync(request.Id);

            if (resolution == null)
            {
                throw new NotFoundException(nameof(Resolution), request.Id);
            }

            if (resolution.IsApproved)
            {
                throw new Exception("Already accepted");
            }
            
            resolution.Version = request.Version;
            resolution.IsApproved = request.IsApproved;
            if (request.File != null)
            {
                // TODO: delete old file
                resolution.Location = await _fileService.UploadAsync(request.File);
            }
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}