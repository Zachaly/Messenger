using FluentValidation;
using Messenger.Application.Command;

namespace Messenger.Application.Validator
{
    public class UpdateUsernameCommandValidator : AbstractValidator<UpdateUsernameCommand>
    {
        public UpdateUsernameCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MinimumLength(5).MaximumLength(100);
        }
    }
}
