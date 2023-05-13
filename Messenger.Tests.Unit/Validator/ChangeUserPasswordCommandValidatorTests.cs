using Messenger.Application.Command;
using Messenger.Application.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Tests.Unit.Validator
{
    public class ChangeUserPasswordCommandValidatorTests : ValidatorTest<ChangeUserPasswordCommandValidator, ChangeUserPasswordCommand>
    {
        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new ChangeUserPasswordCommand
            {
                UserId = 1,
                NewPassword = "zaq1@WSX"
            };

            var res = Validate(command);
            Assert.True(res.IsValid);
        }

        [Fact]
        public void UserIdBelowOne_DoesNotPassValidation()
        {
            var command = new ChangeUserPasswordCommand
            {
                UserId = 0,
                NewPassword = "zaq1@WSX"
            };

            var res = Validate(command);
            Assert.False(res.IsValid);
        }

        [Fact]
        public void NewPasswordEmpty_DoesNotPassValidation()
        {
            var command = new ChangeUserPasswordCommand
            {
                UserId = 1,
                NewPassword = ""
            };

            var res = Validate(command);
            Assert.False(res.IsValid);
        }

        [Fact]
        public void NewPasswordBelowMinimumLength_DoesNotPassValidation()
        {
            var command = new ChangeUserPasswordCommand
            {
                UserId = 1,
                NewPassword = new string('a', 7)
            };

            var res = Validate(command);
            Assert.False(res.IsValid);
        }

        [Fact]
        public void NewPasswordExceedsMaximumLength_DoesNotPassValidation()
        {
            var command = new ChangeUserPasswordCommand
            {
                UserId = 1,
                NewPassword = new string('a', 101)
            };

            var res = Validate(command);
            Assert.False(res.IsValid);
        }
    }
}
