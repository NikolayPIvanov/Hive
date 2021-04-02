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
        private readonly IGigManagementContext _context;

        public DeleteQuestionCommandHandler(IGigManagementContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _context.Questions.FindAsync(request.Id);

            if (question is null)
            {
                throw new NotFoundException(nameof(Question), request.Id);
            }

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}