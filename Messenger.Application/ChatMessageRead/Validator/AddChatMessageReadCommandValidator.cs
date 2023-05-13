using FluentValidation;
using Messenger.Application.Command;

namespace Messenger.Application.Validator
{
    public class AddChatMessageReadCommandValidator : AbstractValidator<AddChatMessageReadCommand>
    {
        public AddChatMessageReadCommandValidator()
        {
            RuleFor(x => x.MessageId).GreaterThan(0);
            RuleFor(x => x.ChatId).GreaterThan(0);
            RuleFor(x => x.UserId).GreaterThan(0);
        }
    }
}
