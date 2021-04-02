using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Common.Core.Mappings;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Contracts.Objects;
using MediatR;

namespace Hive.Gig.Application.Questions.Queries
{
    public record GetQuestionsQuery(int GigId) : IRequest<IEnumerable<QuestionDto>>;

    public class GetQuestionsQueryHandler : IRequestHandler<GetQuestionsQuery, IEnumerable<QuestionDto>>
    {
        private readonly IGigManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetQuestionsQueryHandler(IGigManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<QuestionDto>> Handle(GetQuestionsQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Questions
                .Where(q => q.GigId == request.GigId)
                .ProjectToListAsync<QuestionDto>(_mapper.ConfigurationProvider);
        }
    }
}