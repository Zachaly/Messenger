using Messenger.Application.Command;
using Messenger.Application.Validator;

namespace Messenger.Tests.Unit.Validator
{
    public class UpdateChatCommandValidatorTests : ValidatorTest<UpdateChatCommandValidator, UpdateChatCommand>
    {
        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new UpdateChatCommand
            {
                Id = 1,
                Name = "Test",
            };

            var res = Validate(command);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void IdBelowOne_DoesNotPassValidation()
        {
            var command = new UpdateChatCommand
            {
                Id = 0,
                Name = "Test",
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void NameExceedsMaximumLength_DoesNotPassValidation()
        {
            var command = new UpdateChatCommand
            {
                Id = 1,
                Name = new string('a', 51),
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }
    }
}
