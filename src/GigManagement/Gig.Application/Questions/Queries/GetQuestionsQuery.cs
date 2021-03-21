using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Gig.Contracts;
using Hive.Common.Application.Mappings;
using Hive.Gig.Application.Interfaces;
using MediatR;

namespace Hive.Gig.Application.Questions.Queries
{
    public record GetQuestionsQuery(int GigId) : IRequest<IEnumerable<QuestionDto>>;

    public class GetQuestionsQueryHandler : IRequestHandler<GetQuestionsQuery, IEnumerable<QuestionDto>>
    {
        private readonly IGigManagementContext _context;
        private readonly IMapper _mapper;

        public GetQuestionsQueryHandler(IGigManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<QuestionDto>> Handle(GetQuestionsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Questions
                .Where(q => q.GigId == request.GigId)
                .ProjectToListAsync<QuestionDto>(_mapper.ConfigurationProvider);
        }
    }
}