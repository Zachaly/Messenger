using FluentValidation;
using Messenger.Application.Command;

namespace Messenger.Application.Validator
{
    public class AddDirectMessageCommandValidator : AbstractValidator<AddDirectMessageCommand>
    {
        public AddDirectMessageCommandValidator()
        {
            RuleFor(x => x.Content).NotEmpty().MaximumLength(500);
            RuleFor(x => x.SenderId).GreaterThan(0);
            RuleFor(x => x.ReceiverId).GreaterThan(0);
        }
    }
}
