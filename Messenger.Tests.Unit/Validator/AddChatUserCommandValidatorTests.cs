using Messenger.Application.Command;
using Messenger.Application.Validator;

namespace Messenger.Tests.Unit.Validator
{
    public class AddChatUserCommandValidatorTests : ValidatorTest<AddChatUserCommandValidator, AddChatUserCommand>
    {
        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new AddChatUserCommand
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
            var command = new AddChatUserCommand
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
            var command = new AddChatUserCommand
            {
                UserId = 1,
                ChatId = 0
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }
    }
}
