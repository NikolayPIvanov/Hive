using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Application.Resolutions.Commands
{
    public record UpdateResolutionCommand(int Id, string Version, IFormFile File, bool IsApproved = false) : IRequest;
    
    public class UpdateResolutionCommandValidator : AbstractValidator<CreateResolutionCommand>
    {
        public UpdateResolutionCommandValidator()
        {
            RuleFor(x => x.Version).NotNull();
            RuleFor(x => x.File).Must(x => x.Length > 0);
        }
    }
    
    public class UpdateResolutionCommandHandler : IRequestHandler<UpdateResolutionCommand>
    {
        private readonly IOrderingContext _context;
        private readonly IFileService _fileService;

        public UpdateResolutionCommandHandler(IOrderingContext context, IFileService fileService)
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
                await _fileService.DeleteAsync(resolution.Location);
                resolution.Location = await _fileService.UploadAsync(request.File);
            }
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}