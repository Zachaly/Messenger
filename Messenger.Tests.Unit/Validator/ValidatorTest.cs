using FluentValidation;
using FluentValidation.Results;

namespace Messenger.Tests.Unit.Validator
{
    public abstract class ValidatorTest<TValidator, TCommand> where TValidator: AbstractValidator<TCommand>, new()
    {
        protected ValidationResult Validate(TCommand command) => new TValidator().Validate(command);
    }
}
