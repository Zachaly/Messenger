using FluentValidation;
using Messenger.Application.Command;

namespace Messenger.Application.Validator
{
    public class UpdateChatUserCommandValidator : AbstractValidator<UpdateChatUserCommand>
    {
        public UpdateChatUserCommandValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.ChatId).GreaterThan(0);
        }
    }
}
