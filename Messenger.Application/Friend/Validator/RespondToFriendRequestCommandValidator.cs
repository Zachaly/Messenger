using FluentValidation;
using Messenger.Application.Command;

namespace Messenger.Application.Friend.Validator
{
    public class RespondToFriendRequestCommandValidator : AbstractValidator<RespondToFriendRequestCommand>
    {
        public RespondToFriendRequestCommandValidator()
        {
            RuleFor(x => x.RequestId).GreaterThan(0);
        }
    }
}
