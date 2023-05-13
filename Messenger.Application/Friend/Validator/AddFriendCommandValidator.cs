using FluentValidation;
using Messenger.Application.Command;

namespace Messenger.Application.Validator
{
    public class AddFriendCommandValidator : AbstractValidator<AddFriendCommand>
    {
        public AddFriendCommandValidator()
        {
            RuleFor(x => x.SenderId).GreaterThan(0);
            RuleFor(x => x.ReceiverId).GreaterThan(0);
        }
    }
}
