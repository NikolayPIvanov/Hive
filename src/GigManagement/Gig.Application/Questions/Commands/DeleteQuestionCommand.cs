using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Exceptions;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;

namespace Hive.Gig.Application.Questions.Commands
{
    public record DeleteQuestionCommand(int Id) : IRequest;
    
    public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand>
    {
        private readonly IGigManagementDbContext _dbContext;

        public DeleteQuestionCommandHandler(IGigManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<Unit> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _dbContext.Questions.FindAsync(request.Id);

            if (question is null)
            {
                throw new NotFoundException(nameof(Question), request.Id);
            }

            _dbContext.Questions.Remove(question);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}