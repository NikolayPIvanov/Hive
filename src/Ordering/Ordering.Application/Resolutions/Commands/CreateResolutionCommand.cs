using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Security;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Application.Resolutions.Commands
{
    [Authorize(Roles = "Seller, Administrator")]
    public record CreateResolutionCommand(Guid OrderNumber, string Version, IFormFile File) : IRequest<int>;

    public class CreateResolutionValidator : AbstractValidator<CreateResolutionCommand>
    {
        public CreateResolutionValidator()
        {
            RuleFor(x => x.File)
                .Must(x => x.Length > 0);
        }
    }
    
    public class CreateResolutionCommandHandler : IRequestHandler<CreateResolutionCommand, int>
    {
        private readonly IOrderingContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileService _fileService;

        public CreateResolutionCommandHandler(IOrderingContext context, ICurrentUserService currentUserService, IFileService fileService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _fileService = fileService;
        }
        
        public async Task<int> Handle(CreateResolutionCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.OrderNumber == request.OrderNumber, cancellationToken: cancellationToken);

            if (order == null)
            {
                throw new NotFoundException(nameof(Order), request.OrderNumber);
            }
            
            var location = await _fileService.UploadAsync(request.File);
            var resolution = new Resolution(request.Version, location, order.Id);

            _context.Resolutions.Add(resolution);
            await _context.SaveChangesAsync(cancellationToken);

            return resolution.Id;
        }
    }
}