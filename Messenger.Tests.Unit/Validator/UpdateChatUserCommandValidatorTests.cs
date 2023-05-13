using Messenger.Application.Command;
using Messenger.Application.Validator;

namespace Messenger.Tests.Unit.Validator
{
    public class UpdateChatUserCommandValidatorTests : ValidatorTest<UpdateChatUserCommandValidator, UpdateChatUserCommand>
    {
        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new UpdateChatUserCommand
            {
                UserId = 1,
                ChatId = 2
            };

            var res = Validate(command);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void UserIdBelowOne_DoesNotPassValidation()
        {
            var command = new UpdateChatUserCommand
            {
                UserId = 0,
                ChatId = 2
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void ChatIdBelowOne_DoesNotPassValidation()
        {
            var command = new UpdateChatUserCommand
            {
                UserId = 1,
                ChatId = 0
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }
    }
}
