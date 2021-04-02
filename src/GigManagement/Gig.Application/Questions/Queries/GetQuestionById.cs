using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Contracts.Objects;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Questions.Queries
{
    public record GetQuestionByIdQuery(int Id, int QuestionId) : IRequest<QuestionDto>;
    
    public class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, QuestionDto>
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetQuestionByIdQueryHandler(IGigManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        public async Task<QuestionDto> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Questions.FindAsync(request.QuestionId);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Question), request.QuestionId);
            }

            return _mapper.Map<QuestionDto>(entity);
        }
    }
}