using FluentValidation;
using Messenger.Application.Command;

namespace Messenger.Application.Validator
{
    public class UpdateChatCommandValidator : AbstractValidator<UpdateChatCommand>
    {
        public UpdateChatCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
