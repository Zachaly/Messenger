using Messenger.Application.Command;
using Messenger.Application.Validator;

namespace Messenger.Tests.Unit.Validator
{
    public class SaveDirectMessageImagesCommandValidatorTests : ValidatorTest<SaveDirectMessageImageCommandValidator, SaveDirectMessageImagesCommand>
    {
        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new SaveDirectMessageImagesCommand
            {
                MessageId = 1,
            };

            var res = Validate(command);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void MessageIdBelowOne_DoesNotPassValidation()
        {
            var command = new SaveDirectMessageImagesCommand
            {
                MessageId = 0,
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }
    }
}
