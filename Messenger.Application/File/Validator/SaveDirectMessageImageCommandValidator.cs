using FluentValidation;
using Messenger.Application.Command;

namespace Messenger.Application.Validator
{
    public class SaveDirectMessageImageCommandValidator : AbstractValidator<SaveDirectMessageImagesCommand>
    {
        public SaveDirectMessageImageCommandValidator()
        {
            RuleFor(x => x.MessageId).GreaterThan(0);
        }
    }
}
