using Messenger.Application.Command;
using Messenger.Application.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Tests.Unit.Validator
{
    public class RegisterCommandValidatorTests : ValidatorTest<RegisterCommandValidator, RegisterCommand>
    {
        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new RegisterCommand
            {
                Login = "user login",
                Name = "user name",
                Password = "zaq1@WSX"
            };

            var res = Validate(command);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void LoginEmpty_DoesNotPassValidation()
        {
            var command = new RegisterCommand
            {
                Login = "",
                Name = "user name",
                Password = "zaq1@WSX"
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void NameEmpty_DoesNotPassValidation()
        {
            var command = new RegisterCommand
            {
                Login = "user login",
                Name = "",
                Password = "zaq1@WSX"
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void PasswordEmpty_DoesNotPassValidation()
        {
            var command = new RegisterCommand
            {
                Login = "user login",
                Name = "user name",
                Password = ""
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void LoginBelowMinimumLength_DoesNotPassValidation()
        {
            var command = new RegisterCommand
            {
                Login = new string('a', 4),
                Name = "user name",
                Password = "zaq1@WSX"
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void NameBelowMinimumLength_DoesNotPassValidation()
        {
            var command = new RegisterCommand
            {
                Login = "user login",
                Name = new string('a', 4),
                Password = "zaq1@WSX"
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void PasswordBelowMinimumLength_DoesNotPassValidation()
        {
            var command = new RegisterCommand
            {
                Login = "user login",
                Name = "user name",
                Password = new string('a', 7)
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void LoginExceedsMaximumLength_DoesNotPassValidation()
        {
            var command = new RegisterCommand
            {
                Login = new string('a', 101),
                Name = "user name",
                Password = "zaq1@WSX"
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void NameExceedsMaximumLength_DoesNotPassValidation()
        {
            var command = new RegisterCommand
            {
                Login = "user login",
                Name = new string('a', 101),
                Password = "zaq1@WSX"
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void PasswordExceedsMaximumLength_DoesNotPassValidation()
        {
            var command = new RegisterCommand
            {
                Login = "user login",
                Name = "user name",
                Password = new string('a', 101)
            };

            var res = Validate(command);

            Assert.False(res.IsValid);
        }
    }
}
