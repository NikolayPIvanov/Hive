﻿using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hive.Common.Application.Exceptions;
using Hive.Gig.Application.Interfaces;
using Hive.Gig.Domain.Entities;
using MediatR;

namespace Hive.Gig.Application.Questions.Commands
{
    
    public record UpdateQuestionCommand(int Id, string Question, string Answer) : IRequest;

    public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
    {
        public UpdateQuestionCommandValidator(IGigManagementContext context)
        {
            RuleFor(x => x.Question)
                .MaximumLength(50).WithMessage("{PropertyName} cannot have more than {MaxLength} characters.")
                .MinimumLength(3).WithMessage("{PropertyName} cannot have less than {MinLength} characters.")
                .NotEmpty().WithMessage("A {PropertyName} must be provided");
            
            RuleFor(x => x.Answer)
                .MaximumLength(1000).WithMessage("{PropertyName} cannot have more than {MaxLength} characters.")
                .MinimumLength(3).WithMessage("{PropertyName} cannot have less than {MinLength} characters.")
                .NotEmpty().WithMessage("A {PropertyName} must be provided");
        }
    }
    
    public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand>
    {
        private readonly IGigManagementContext _context;
        
        public UpdateQuestionCommandHandler(IGigManagementContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _context.Questions.FindAsync(request.Id);

            if (question is null)
            {
                throw new NotFoundException(nameof(Question), request.Id);
            }

            question.Answer = request.Answer;
            question.Title = request.Question;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}