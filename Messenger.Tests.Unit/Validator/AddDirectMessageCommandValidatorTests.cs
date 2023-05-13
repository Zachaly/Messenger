using Messenger.Application.Command;
using Messenger.Application.Validator;

namespace Messenger.Tests.Unit.Validator
{
    public class AddDirectMessageCommandValidatorTests : ValidatorTest<AddDirectMessageCommandValidator, AddDirectMessageCommand>
    {
        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new AddDirectMessageCommand
            {
                Content = "con",
                ReceiverId = 1,
                SenderId = 2
            };

            var res = Validate(command);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void ContentEmpty_DoesNotPassValidation()
        {
            var command = new AddDirectMessageCommand
            {
                Content = "",
                ReceiverId = 1,
                SenderId = 2
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void ContentExceedsMaximumLength_DoesNotPassValidation()
        {
            var command = new AddDirectMessageCommand
            {
                Content = new string('a', 501),
                ReceiverId = 1,
                SenderId = 2
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void ReceiverIdBelowOne_DoesNotPassValidation()
        {
            var command = new AddDirectMessageCommand
            {
                Content = "con",
                ReceiverId = 0,
                SenderId = 2
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void SenderIdBelowOne_DoesNotPassValidation()
        {
            var command = new AddDirectMessageCommand
            {
                Content = "con",
                ReceiverId = 1,
                SenderId = 0
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }
    }
}
