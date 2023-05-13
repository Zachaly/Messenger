using FluentValidation;
using Messenger.Application.Command;

namespace Messenger.Application.Validator
{
    public class ChangeUserPasswordCommandValidator : AbstractValidator<ChangeUserPasswordCommand>
    {
        public ChangeUserPasswordCommandValidator()
        {
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(8).MaximumLength(100);
            RuleFor(x => x.UserId).GreaterThan(0);
        }
    }
}
