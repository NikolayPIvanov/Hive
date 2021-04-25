using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.Gig.Application.Reviews.Queries
{
    public record ReviewDto(string Comment, double Rating);
    
    public record GetReviewByIdQuery(int GigId, int ReviewId) : IRequest<ReviewDto>;
    
    public class GetReviewByIdQueryHandler : IRequestHandler<GetReviewByIdQuery, ReviewDto>
    {
        private readonly IGigManagementDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetReviewByIdQueryHandler> _logger;


        public GetReviewByIdQueryHandler(IGigManagementDbContext context, IMapper mapper, ILogger<GetReviewByIdQueryHandler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<ReviewDto> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            var review =
                await _context.Reviews.FirstOrDefaultAsync(x => x.GigId == request.GigId && x.Id == request.ReviewId,
                    cancellationToken);

            if (review == null)
            {
                _logger.LogWarning("Gig with id: {@Id} was not found", request.GigId);
                throw new NotFoundException(nameof(Review), request.ReviewId);
            }

            return _mapper.Map<ReviewDto>(review);
        }
    }
}