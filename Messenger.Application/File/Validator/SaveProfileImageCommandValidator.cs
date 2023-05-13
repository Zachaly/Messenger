using FluentValidation;
using Messenger.Application.Command;

namespace Messenger.Application.Validator
{
    public class SaveProfileImageCommandValidator : AbstractValidator<SaveProfileImageCommand>
    {
        public SaveProfileImageCommandValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
        }
    }
}
