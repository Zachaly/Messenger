using FluentValidation;
using Messenger.Application.Command;

namespace Messenger.Application.Validator
{
    public class AddMessageReportCommandValidator : AbstractValidator<AddMessageReportCommand>
    {
        public AddMessageReportCommandValidator() 
        {
            RuleFor(x => x.ReportedUserId).GreaterThan(0);
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.Reason).NotEmpty().MaximumLength(100);
            RuleFor(x => x.MessageId).GreaterThan(0);
        }
    }
}
