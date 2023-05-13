using FluentValidation;
using Messenger.Application.Command;

namespace Messenger.Application.Validator
{
    public class AddChatCommandValidator : AbstractValidator<AddChatCommand>
    {
        public AddChatCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.UserId).GreaterThan(0);
        }
    }
}
