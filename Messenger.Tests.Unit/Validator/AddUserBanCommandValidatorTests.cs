using Messenger.Application.Command;
using Messenger.Application.Validator;

namespace Messenger.Tests.Unit.Validator
{
    public class AddUserBanCommandValidatorTests : ValidatorTest<AddUserBanCommandValidator, AddUserBanCommand>
    {
        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new AddUserBanCommand
            {
                UserId = 1,
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1),
            };

            var res = Validate(command);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void UserIdBelowOne_DoesNotPassValidation()
        {
            var command = new AddUserBanCommand
            {
                UserId = 0,
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1),
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void StartInFuture_DoesNotPassValidation()
        {
            var command = new AddUserBanCommand
            {
                UserId = 1,
                Start = DateTime.Now.AddDays(1),
                End = DateTime.Now.AddDays(1),
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void EndInPast_DoesNotPassValidation()
        {
            var command = new AddUserBanCommand
            {
                UserId = 1,
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(-1),
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }
    }
}
