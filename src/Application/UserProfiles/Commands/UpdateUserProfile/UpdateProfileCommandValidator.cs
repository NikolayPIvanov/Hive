using FluentValidation;

namespace Hive.Application.UserProfiles.Commands.UpdateUserProfile
{
    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand.Command>
    {
        public UpdateProfileCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .MaximumLength(100);
            // TODO:
        }
    }
}