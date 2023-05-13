using FluentValidation;
using Messenger.Application.Command;

namespace Messenger.Application.Validator
{
    public class AddUserClaimCommandValidator : AbstractValidator<AddUserClaimCommand>
    {
        public AddUserClaimCommandValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.Value).NotEmpty();
        }
    }
}
