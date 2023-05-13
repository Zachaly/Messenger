using FluentValidation;
using Messenger.Application.Command;

namespace Messenger.Application.Validator
{
    public class AddDirectMessageReactionCommandValidator : AbstractValidator<AddDirectMessageReactionCommand>
    {
        public AddDirectMessageReactionCommandValidator()
        {
            RuleFor(x => x.MessageId).GreaterThan(0);
            RuleFor(x => x.Reaction).NotEmpty().MaximumLength(2);
            RuleFor(x => x.ReceiverId).GreaterThan(0);
        }
    }
}
