using Messenger.Application.Command;
using Messenger.Application.Validator;

namespace Messenger.Tests.Unit.Validator
{
    public class AddChatCommandValidatorTests : ValidatorTest<AddChatCommandValidator, AddChatCommand>
    {
        

        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new AddChatCommand
            {
                Name = "Test",
                UserId = 1
            };

            var res = Validate(command);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void UserIdLessThanOne_DoesNotPassValidation()
        {
            var command = new AddChatCommand
            {
                UserId = 0,
                Name = "test"
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void NameEmpty_DoesNotPassValidation()
        {
            var command = new AddChatCommand
            {
                UserId = 1,
                Name = ""
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void NameExceedsMaximumLength_DoesNotPassValidation()
        {
            var command = new AddChatCommand
            {
                UserId = 1,
                Name = new string('a', 51)
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }
    }
}
