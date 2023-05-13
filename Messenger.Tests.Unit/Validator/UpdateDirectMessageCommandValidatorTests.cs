using Messenger.Application.Command;
using Messenger.Application.Validator;

namespace Messenger.Tests.Unit.Validator
{
    public class UpdateDirectMessageCommandValidatorTests : ValidatorTest<UpdateDirectMessageCommandValidator, UpdateDirectMessageCommand>
    {
        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new UpdateDirectMessageCommand
            {
                Id = 1
            };

            var res = Validate(command);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void IdBelowOne_DoesNotPassValidation()
        {
            var command = new UpdateDirectMessageCommand
            {
                Id = 0
            };

            var res = Validate(command);
            
            Assert.False(res.IsValid);
        }
    }
}
