using Messenger.Application.Command;
using Messenger.Application.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Tests.Unit.Validator
{
    public class AddUserClaimCommandValidatorTests : ValidatorTest<AddUserClaimCommandValidator, AddUserClaimCommand>
    {
        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new AddUserClaimCommand
            {
                UserId = 1,
                Value = "val"
            };

            var res = Validate(command);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void UserIdBelowOne_DoesNotPassValidation()
        {
            var command = new AddUserClaimCommand
            {
                UserId = 0,
                Value = "val"
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void ValueEmpty_DoesNotPassValidation()
        {
            var command = new AddUserClaimCommand
            {
                UserId = 1,
                Value = ""
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }
    }
}
