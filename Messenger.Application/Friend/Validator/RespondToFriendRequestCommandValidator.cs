using FluentValidation;
using Messenger.Application.Command;

namespace Messenger.Application.Validator
{
    public class RespondToFriendRequestCommandValidator : AbstractValidator<RespondToFriendRequestCommand>
    {
        public RespondToFriendRequestCommandValidator()
        {
            RuleFor(x => x.RequestId).GreaterThan(0);
        }
    }
}
