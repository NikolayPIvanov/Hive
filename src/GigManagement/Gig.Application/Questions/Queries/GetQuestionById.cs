using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Gig.Contracts;
using Hive.Common.Application.Exceptions;
using Hive.Common.Application.Mappings;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Questions.Queries
{
    public record GetQuestionByIdQuery(int Id, int QuestionId) : IRequest<QuestionDto>;
    
    public class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, QuestionDto>
    {
        private readonly IGigManagementContext _context;
        private readonly IMapper _mapper;

        public GetQuestionByIdQueryHandler(IGigManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<QuestionDto> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Questions.FindAsync(request.QuestionId);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Question), request.QuestionId);
            }

            return _mapper.Map<QuestionDto>(entity);
        }
    }
}