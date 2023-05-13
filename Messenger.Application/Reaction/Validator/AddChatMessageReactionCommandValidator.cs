using FluentValidation;
using Messenger.Application.Command;

namespace Messenger.Application.Validator
{
    public class AddChatMessageReactionCommandValidator : AbstractValidator<AddChatMessageReactionCommand>
    {
        public AddChatMessageReactionCommandValidator()
        {
            RuleFor(x => x.MessageId).GreaterThan(0);
            RuleFor(x => x.ChatId).GreaterThan(0);
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.Reaction).NotEmpty().MaximumLength(2);
        }
    }
}
