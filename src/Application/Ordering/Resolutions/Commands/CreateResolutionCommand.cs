using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Domain.Entities.Orders;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Ordering.Resolutions.Commands
{
    public record CreateResolutionCommand(Guid OrderNumber, string Version, IFormFile File) : IRequest<int>;

    public class CreateResolutionCommandHandler : IRequestHandler<CreateResolutionCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileService _fileService;

        public CreateResolutionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IFileService fileService)
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

            if (request.File.Length == 0)
            {
                // throw bad request;
            }
            
            var location = await _fileService.UploadAsync(request.File);
            
            var resolution = new Resolution(request.Version, location, order.Id);

            _context.Resolutions.Add(resolution);
            await _context.SaveChangesAsync(cancellationToken);

            return resolution.Id;
        }
    }
}