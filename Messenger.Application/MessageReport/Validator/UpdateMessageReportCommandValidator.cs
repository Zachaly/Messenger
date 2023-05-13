using FluentValidation;
using Messenger.Application.Command;

namespace Messenger.Application.Validator
{
    public class UpdateMessageReportCommandValidator : AbstractValidator<UpdateMessageReportCommand>
    {
        public UpdateMessageReportCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Reason).MaximumLength(100);
            RuleFor(x => x.AttachedMessageId).GreaterThan(0);
        }
    }
}
