using Messenger.Application.Command;
using Messenger.Application.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Tests.Unit.Validator
{
    public class UpdateUsernameCommandValidatorTests : ValidatorTest<UpdateUsernameCommandValidator, UpdateUsernameCommand>
    {
        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new UpdateUsernameCommand
            {
                Id = 1,
                Name = "username"
            };

            var res = Validate(command);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void IdBelowOne_DoesNotPassValidation()
        {
            var command = new UpdateUsernameCommand
            {
                Id = 0,
                Name = "username"
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void NameEmpty_DoesNotPassValidation()
        {
            var command = new UpdateUsernameCommand
            {
                Id = 1,
                Name = ""
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void NameExceedsMaximumLength_DoesNotPassValidation()
        {
            var command = new UpdateUsernameCommand
            {
                Id = 1,
                Name = new string('a', 101)
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void NameBelowMinimumLength_DoesNotPassValidation()
        {
            var command = new UpdateUsernameCommand
            {
                Id = 1,
                Name = new string('a', 4)
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }
    }
}
