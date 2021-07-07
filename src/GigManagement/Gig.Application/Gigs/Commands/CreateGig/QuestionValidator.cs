using FluentValidation;

namespace Hive.Gig.Application.Gigs.Commands
{
    public class QuestionValidator : AbstractValidator<QuestionModel>
    {
        public QuestionValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(100).WithMessage("Title length must not be above 100 characters.")
                .MinimumLength(3).WithMessage("Title length must not be below 3 characters.")
                .NotEmpty().WithMessage("Title should be provided.");
            
            RuleFor(x => x.Answer)
                .MaximumLength(1000).WithMessage("Answer length must not be above 1000 characters.")
                .MinimumLength(3).WithMessage("Answer length must not be below 3 characters.")
                .NotEmpty().WithMessage("Answer should be provided.");
        }
    }
}