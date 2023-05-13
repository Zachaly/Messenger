using Messenger.Application.Command;
using Messenger.Application.Validator;

namespace Messenger.Tests.Unit.Validator
{
    public class SaveChatMessageImageValidatorTests : ValidatorTest<SaveChatMessageImageCommandValidator, SaveChatMessageImageCommand>
    {
        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new SaveChatMessageImageCommand
            {
                MessageId = 1,
            };

            var res = Validate(command);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void MessageIdBelowOne_DoesNotPassValidation()
        {
            var command = new SaveChatMessageImageCommand
            {
                MessageId = 0,
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }
    }
}
