using FluentValidation;
using Messenger.Application.Command;

namespace Messenger.Application.Validator
{
    public class AddChatUserCommandValidator : AbstractValidator<AddChatUserCommand>
    {
        public AddChatUserCommandValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.ChatId).GreaterThan(0);
        }
    }
}
