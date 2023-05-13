using Messenger.Application.Command;
using Messenger.Application.Validator;

namespace Messenger.Tests.Unit.Validator
{
    public class AddFriendCommandValidatorTests : ValidatorTest<AddFriendCommandValidator, AddFriendCommand>
    {
        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new AddFriendCommand
            {
                ReceiverId = 1,
                SenderId = 2,
            };

            var res = Validate(command);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void ReceverIdBelowOne_DoesNotPassValidation()
        {
            var command = new AddFriendCommand
            {
                ReceiverId = 0,
                SenderId = 2,
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void SenderIdBelowOne_DoesNotPassValidation()
        {
            var command = new AddFriendCommand
            {
                ReceiverId = 1,
                SenderId = 0,
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }
    }
}
