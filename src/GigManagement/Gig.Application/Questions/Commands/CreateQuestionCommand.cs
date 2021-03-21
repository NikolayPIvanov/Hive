using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Gig.Application.Questions.Commands
{
    public record CreateQuestionCommand(string Question, string Answer, int GigId) : IRequest<int>;

    public class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
    {
        public CreateQuestionCommandValidator(IGigManagementContext context)
        {
            RuleFor(x => x.Question)
                .MaximumLength(50).WithMessage("{PropertyName} cannot have more than {MaxLength} characters.")
                .MinimumLength(3).WithMessage("{PropertyName} cannot have less than {MinLength} characters.")
                .NotEmpty().WithMessage("A {PropertyName} must be provided");
            
            RuleFor(x => x.Answer)
                .MaximumLength(1000).WithMessage("{PropertyName} cannot have more than {MaxLength} characters.")
                .MinimumLength(3).WithMessage("{PropertyName} cannot have less than {MinLength} characters.")
                .NotEmpty().WithMessage("A {PropertyName} must be provided");
            
            RuleFor(x => x.GigId)
                .MustAsync(async (id, token) => await context.Gigs.AnyAsync(x => x.Id == id, token))
                .WithMessage("Must specify a valid gig.");
        }
    }
    
    public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, int>
    {
        private readonly IGigManagementContext _context;

        public CreateQuestionCommandHandler(IGigManagementContext context)
        {
            _context = context;
        }
        
        public async Task<int> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = new Question(request.Question, request.Answer, request.GigId);

            _context.Questions.Add(question);
            await _context.SaveChangesAsync(cancellationToken);

            return question.Id;
        }
    }
}