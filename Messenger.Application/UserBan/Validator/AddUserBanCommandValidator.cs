using FluentValidation;
using Messenger.Application.Command;

namespace Messenger.Application.Validator
{
    public class AddUserBanCommandValidator : AbstractValidator<AddUserBanCommand>
    {
        public AddUserBanCommandValidator()
        {
            RuleFor(x => x.End).GreaterThan(DateTime.Now);
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.Start).LessThan(DateTime.Now);
        }
    }
}
