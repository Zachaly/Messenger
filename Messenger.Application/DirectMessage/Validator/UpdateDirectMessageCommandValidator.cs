using FluentValidation;
using Messenger.Application.Command;

namespace Messenger.Application.Validator
{
    public class UpdateDirectMessageCommandValidator : AbstractValidator<UpdateDirectMessageCommand>
    {
        public UpdateDirectMessageCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
