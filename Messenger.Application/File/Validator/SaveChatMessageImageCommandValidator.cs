using FluentValidation;
using Messenger.Application.Command;

namespace Messenger.Application.Validator
{
    public class SaveChatMessageImageCommandValidator : AbstractValidator<SaveChatMessageImageCommand>
    {
        public SaveChatMessageImageCommandValidator()
        {
            RuleFor(x => x.MessageId).GreaterThan(0);
        }
    }
}
