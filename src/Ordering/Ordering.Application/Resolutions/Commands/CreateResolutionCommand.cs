using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using FluentValidation;
using Hive.Common.Core.Exceptions;
using Hive.Common.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Application.Resolutions.Commands
{
    public record CreateResolutionCommand(Guid OrderNumber, string Version, string Extension, Stream FileStream) : IRequest<int>;

    public class CreateResolutionValidator : AbstractValidator<CreateResolutionCommand>
    {
        public CreateResolutionValidator(IOrderingContext context)
        {
            RuleFor(x => x.Version)
                .MustAsync(async (command, version, token) =>
                {
                    var order = await context.Orders
                        .Include(o => o.Resolutions.Where(r => r.Version == version))
                        .FirstOrDefaultAsync(x => x.OrderNumber == command.OrderNumber, token);
                    if (order == null)
                        return false;

                    var versionIsUsed = order.Resolutions.Any();

                    return !versionIsUsed;
                })
                .WithMessage("Provide {Property} that is not used.");
        }
    }
    
    public class CreateResolutionCommandHandler : IRequestHandler<CreateResolutionCommand, int>
    {
        private readonly IOrderingContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileService _fileService;
        private readonly ILogger<CreateResolutionCommandHandler> _logger;

        public CreateResolutionCommandHandler(IOrderingContext context, ICurrentUserService currentUserService, IFileService fileService,
            ILogger<CreateResolutionCommandHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<int> Handle(CreateResolutionCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(x => x.OrderNumber == request.OrderNumber, cancellationToken: cancellationToken);
            
            if (order == null)
            {
                _logger.LogWarning("Order with number: {@Id} was not found", request.OrderNumber);
                throw new NotFoundException(nameof(Order), request.OrderNumber);
            }
            
            var location = await _fileService.UploadAsync("resolutions", request.FileStream, request.Extension, cancellationToken);
            var resolution = new Resolution(request.Version, location, order.Id);

            _context.Resolutions.Add(resolution);
            await _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("Resolution was created for order with number: {@Id}", request.OrderNumber);

            return resolution.Id;
        }
    }
}