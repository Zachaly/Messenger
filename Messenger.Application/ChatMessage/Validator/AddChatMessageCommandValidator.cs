using FluentValidation;
using Messenger.Application.Command;

namespace Messenger.Application.Validator
{
    public class AddChatMessageCommandValidator : AbstractValidator<AddChatMessageCommand>
    {
        public AddChatMessageCommandValidator()
        {
            RuleFor(x => x.ChatId).GreaterThan(0);
            RuleFor(x => x.SenderId).GreaterThan(0);
            RuleFor(x => x.Content).NotEmpty().MaximumLength(500);
        }
    }
}
