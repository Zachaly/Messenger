using FluentValidation;
using Messenger.Application.Command;

namespace Messenger.Application.Validator
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Password).NotEmpty().MaximumLength(6).MaximumLength(100);
            RuleFor(x => x.Name).NotEmpty().MinimumLength(5).MaximumLength(100);
            RuleFor(x => x.Login).NotEmpty().MinimumLength(5).MaximumLength(100);
        }
    }
}
